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
			var sourceFiles = LoadProjectDocuments();

			ReadProjectDocuments(sourceFiles);

			var targetFiles = GenerateTargetFilesList(sourceFiles);

			ConsolidateAndStripGlobalUsings(targetFiles);

			GeneratePackedLibraryFiles(targetFiles);

			return 0;
		}

		IEnumerable<SrcDoc> LoadProjectDocuments()
		{
			if(PackOptions.Quiet == false)
			{
				Console.WriteLine($"Reading project file {PackOptions.ProjectFile}");
			}

			var projectfilepath = PackOptions.ProjectFile;
			//normalize path?

			Microsoft.Build.Locator.MSBuildLocator.RegisterDefaults();

			var workspace = MSBuildWorkspace.Create();
			var project = workspace.OpenProjectAsync(projectfilepath).Result;

			var documents = project.Documents.Where(_ => _.SupportsSyntaxTree == true);

			List<SrcDoc> srcdocs = new List<SrcDoc>();
			foreach(var document in documents)
			{
				bool skip = false;
				if(document.Name == "AssemblyInfo.cs") skip = true;
				else if(document.Name.EndsWith("AssemblyAttributes.cs")) skip = true;

				if(skip==true)
				{
					if(PackOptions.Verbose && PackOptions.Quiet==false)
						Console.WriteLine($"skip {document.Name}");
					continue;
				}

				if(PackOptions.Verbose)
				{
					Console.WriteLine(document.Name);
				}

				srcdocs.Add(new SrcDoc() {
					Document = document
				});
			}

			return srcdocs;
		}

		void ReadProjectDocuments(IEnumerable<SrcDoc> srcdocList)
		{
			if(PackOptions.Verbose)
				Console.WriteLine("ReadProjectDocuments:");

			List<SrcDoc> docs = new List<SrcDoc>(srcdocList.Count());

			foreach(var srcdoc in srcdocList)
			{
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

			//return docs;
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

				if(targetfileDirective == null)
				{
					targetfileDirective = new PackerDirectiveNode {
						Name = "targetFile",
						Options = PackOptions.TargetFile ?? "lib.cs"
					};
				}

				if(targetfiles.ContainsKey(targetfileDirective.Options) == false)
				{
					//add new targetfile to targetfiles list
					var targetfile = new TargetFile(targetfileDirective.Options);
					targetfiles.Add(targetfileDirective.Options, targetfile);

					if(PackOptions.Verbose)
					{
						Console.WriteLine($"New target file: {targetfile.Name}");
					}
				}

				//add this sourcedoc to targetfile
				targetfiles[targetfileDirective.Options].SourceDocs.Add(srcdoc);

			}

			return targetfiles.Values;
		}

		private void ConsolidateAndStripGlobalUsings(IEnumerable<TargetFile> targetFiles)
		{
			//var usingsrewriter = new UsingsConsolidateAndStrip();
			//usingsrewriter.CombineGlobalUsings(targetFiles.SelectMany(_ => _.SourceDocs).ToArray());

			foreach(var targetfile in targetFiles)
			{
				var globalnamespaces = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

				foreach(var srcdoc in targetfile.SourceDocs)
				{
					var usingswalker = new UsingsConsolidateAndStrip();
					var usingDirectivesList = usingswalker.FindUsings(srcdoc);
					
					if(PackOptions.Verbose)
					{
						Console.WriteLine("\nUsings in {0}:", srcdoc.Document.Name);
						Console.WriteLine(String.Join("\n", usingDirectivesList.Select(_ => _.Name.ToString())));
					}

					foreach(var usingdir in usingDirectivesList.Select(_ => _.Name.ToString()))
					{
						if(globalnamespaces.Contains(usingdir) == false)
							globalnamespaces.Add(usingdir);
					}

					var newsyntaxtree = usingswalker.RemoveTopLevelUsings(srcdoc);
					srcdoc.SyntaxTree = newsyntaxtree.SyntaxTree;
				}

				targetfile.GlobalUsings = globalnamespaces.OrderBy(_=>_).ToList();
			}
		}

	
		public void GeneratePackedLibraryFiles(IEnumerable<TargetFile> targetfilesList)
		{
			string projectname = PackOptions_GetProjectName();
			string version = PackOptions_GetVersion();

			string baseName = $"_{projectname},{version},";

			if(PackOptions.Quiet == false) { Console.WriteLine("Build output"); }

			if(string.IsNullOrEmpty(this.PackOptions.OutputPath) == true)
			{
				this.PackOptions.OutputPath = ".\\";
			}

			if(PackOptions.Quiet == false)
				Console.WriteLine($"Writing files to '{this.PackOptions.OutputPath}'.");

			foreach(var targetfile in targetfilesList)
			{
				try
				{
					TargetFileWriter g = new TargetFileWriter(this.PackOptions.OutputPath, baseName);
					g.Write(targetfile);

					if(PackOptions.Quiet == false)
						Console.WriteLine($"Wrote {baseName + targetfile.Name}");
				}
				catch(Exception ex)
				{
					Console.Error.WriteLine($"Failed to write {targetfile.Name}: " + ex.ToString());
				}
			}
		}

		internal string PackOptions_GetProjectName()
		{
			if(string.IsNullOrEmpty(this.PackOptions.ProjectName)==false)
			{
				return this.PackOptions.ProjectName;
			}

			return Path.GetFileNameWithoutExtension(this.PackOptions.ProjectFile);
		}

		internal string PackOptions_GetVersion()
		{
			if(string.IsNullOrEmpty(this.PackOptions.Version)==false)
			{
				return this.PackOptions.Version;
			}

			//throw new NotImplementedException("validation shouldve caught Version not set.");
			return DateTime.UtcNow.ToString("yyyyMMddhhmm");
		}

		private void writeverbose(Func<string> messageFunc)
		{
			if(PackOptions.Verbose) Console.WriteLine(messageFunc());
		}

	}
}
