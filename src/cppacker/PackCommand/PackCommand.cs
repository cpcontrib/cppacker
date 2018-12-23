using cppacker.Parsing;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace cppacker.Pack
{
	public class PackCommand : CommandLine.ICommand, CommandLine.ICommandOptionsValidator
	{
		public CommandLine.OptionsValidation Validate()
		{
			var validation = new CommandLine.OptionsValidation();

			if(File.Exists(PackOptions.ProjectFile) == false)
			{
				validation.AddMessage("ProjectFile", $"The file specified '{PackOptions.ProjectFile}' was not found.");
			}

			return validation;
		}
		private bool? _isValid;
		public bool IsValid()
		{
			if(_isValid == null)
			{
				var validation = this.Validate();
				_isValid = validation.IsValid;
			}
			return _isValid.GetValueOrDefault();
		}

		public PackCommand(PackOptions options)
		{
			if(options == null) throw new ArgumentNullException("options");
			this.PackOptions = options;
		}

		private PackOptions PackOptions;
		private Project Project;

		public int Execute()
		{
			var sourceFiles = LoadSrcDocList();

			var targetFiles = GenerateTargetFilesList(sourceFiles);

			ConsolidateGlobalUsings(sourceFiles, targetFiles);

			GeneratePackedLibraryFiles(targetFiles);

			return 0;
		}

		IEnumerable<SrcDoc> LoadProject()
		{
			var projectfilepath = PackOptions.ProjectFile;
			//normalize path?

			Microsoft.Build.Locator.MSBuildLocator.RegisterDefaults();

			var workspace = MSBuildWorkspace.Create();
			var project = workspace.OpenProjectAsync(projectfilepath).Result;

			var documents = project.Documents.Where(_ => _.SupportsSyntaxTree == true).Select(item =>
				new SrcDoc() {
					Document = item
				}
			);

			return documents;
		}

		IEnumerable<SrcDoc> LoadSrcDocList()
		{
			var srcdocList = LoadProject();

			List<SrcDoc> docs = new List<SrcDoc>(srcdocList.Count());

			foreach(var srcdoc in srcdocList)
			{
				if(srcdoc.Document.Name == "AssemblyInfo.cs") continue;

				var packerDirectives = GetPackerDirectives(srcdoc.Document);

				if(packerDirectives.Count() > 0 && packerDirectives.Any(_ => _.Name == "exclude") == true)
				{
					writeverbose(() => $"skip {srcdoc.Document.Name}"); 
				}
				else
				{
					srcdoc.PackerDirectives = packerDirectives;
					srcdoc.SyntaxTree = srcdoc.Document.GetSyntaxTreeAsync().Result;

					docs.Add(srcdoc);
					writeverbose(() => $" ok  {srcdoc.Document.Name}");
				}
			}

			return docs;
		}

		public IEnumerable<PackerDirectiveNode> GetPackerDirectives(Document document)
		{
			StringWriter sw = new StringWriter();
			document.GetTextAsync().Result.Write(sw);
			var stringlines = sw.ToString().Split('\n');

			PackerDirectivesParser p = new PackerDirectivesParser();
			return p.ParseLines(stringlines);
		}

		IEnumerable<TargetFile> GenerateTargetFilesList(IEnumerable<SrcDoc> sourceFiles)
		{
			Dictionary<string,TargetFile> targetfiles = new Dictionary<string,TargetFile>(StringComparer.OrdinalIgnoreCase);

			foreach(var srcdoc in sourceFiles)
			{
				PackerDirectiveNode targetfileDirective = null;

				try { targetfileDirective = srcdoc.PackerDirectives.SingleOrDefault(_ => _.Name == "targetFile"); }
				catch(InvalidOperationException ex)
				{
					throw new ApplicationException(
						$"The file '{srcdoc.Document.Name}' has multiple targetfile packer directives.",
						ex);
				}

				string targetFileName = "lib.cs";

				if(targetfileDirective != null)
				{
					targetFileName = targetfileDirective.Options;
				}

				if(targetfiles.ContainsKey(targetFileName) == false)
				{
					//add new targetfile to targetfiles list
					var targetfile = new TargetFile(targetfileDirective.Options);
					targetfiles.Add(targetFileName, targetfile);
				}

				//add this sourcedoc to targetfile
				targetfiles[targetFileName].SourceDocs.Add(srcdoc);

			}

			return targetfiles.Values;
		}

		private void ConsolidateGlobalUsings(IEnumerable<SrcDoc> sourceFiles, IEnumerable<TargetFile> targetFiles)
		{

		}


		public void GeneratePackedLibraryFiles(IEnumerable<TargetFile> targetfilesList)
		{
			PackOptions.ProjectName = PackOptions.ProjectName ?? "Unnamed";
			PackOptions.Version = PackOptions.Version ?? "0.0.0";

			string baseName = $"_{PackOptions.ProjectName},{PackOptions.Version},";

			if(PackOptions.Quiet == false) { Console.WriteLine("Build output"); }

			if(string.IsNullOrEmpty(this.PackOptions.OutputPath) == true)
			{
				this.PackOptions.OutputPath = ".\\";
			}

			foreach(var targetfile in targetfilesList)
			{
				TargetFileWriter g = new TargetFileWriter(this.PackOptions.OutputPath, baseName);
				g.Write(targetfile);
			}
		}

		private void writeverbose(Func<string> messageFunc)
		{
			if(PackOptions.Verbose) Console.WriteLine(messageFunc());
		}

	}
}
