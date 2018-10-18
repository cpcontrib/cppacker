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
	public class PackCommand : CommandLine.ICommand
	{
		public CommandLine.OptionsValidation ValidateOptions(CommandLine.ICommandOptions options)
		{
			PackOptions PackOptions = (PackOptions)options;
			var validation = new CommandLine.OptionsValidation();

			if(File.Exists(PackOptions.ProjectFile) == false)
			{
				validation.AddMessage("ProjectFile", $"The file specified '{PackOptions.ProjectFile}' was not found.");
			}

			return validation;
		}

		public PackCommand(PackOptions options)
		{
			this.PackOptions = options;
		}

		public PackCommand()
		{
		}

		private PackOptions PackOptions;
		private Project Project;

		public int Execute()
		{
			var project = LoadProject();

			var documents = GetDocumentsForBuild(project);

			PackOptions.ProjectName = PackOptions.ProjectName ?? "Unnamed";
			PackOptions.Version = PackOptions.Version ?? "0.0.0";
			
			BuildOutput(documents);

			return 0;
		}

		Project LoadProject()
		{
			var projectfilepath = PackOptions.ProjectFile;
			//normalize path?

			var workspace = MSBuildWorkspace.Create();
			return workspace.OpenProjectAsync(projectfilepath).Result;
		}

		IEnumerable<PackDocument> GetDocumentsForBuild(Project project)
		{
			List<PackDocument> documentsForBuild = new List<PackDocument>();

			var documents = project.Documents.Where(_ => _.SupportsSyntaxTree == true).ToList();
			foreach(var document in documents)
			{
				var text = File.ReadAllText(document.FilePath);

				var packerDirectives = GetPackerDirectives(document);

				if(packerDirectives.Count() > 0 && packerDirectives.Any(_ => _.Name == "ignore") == true)
				{
					writeverbose(() => $"skip {document.Name}"); 
				}
				else
				{
					writeverbose(() => $" ok  {document.Name}");
					documentsForBuild.Add(new PackDocument { Document = document, PackerDirectives = packerDirectives });
				}
			}

			return documentsForBuild;
		}

		public IEnumerable<PackerDirective> GetPackerDirectives(Document document)
		{
			Regex packerDirectivesRegex = new Regex(@"//!packer:([A-Za-z\-]*)(.*)");

			string text = File.ReadAllText(document.FilePath);

			List<PackerDirective> packerDirectives = new List<PackerDirective>();
			foreach(Match match in packerDirectivesRegex.Matches(text))
			{
				if(match.Success)
				{
					packerDirectives.Add(new PackerDirective { Name = match.Groups[1].Value, Options = match.Groups[2].Value });
				}
			}

			return packerDirectives;
		}

		public void BuildOutput(IEnumerable<PackDocument> documents)
		{
			string baseName = $"_{PackOptions.ProjectName};{PackOptions.Version};";

			if(PackOptions.Quiet == false) { Console.WriteLine("Build output"); }

			foreach(var packdocument in documents)
			{
				
			}
		}

		private void writeverbose(Func<string> messageFunc)
		{
			if(PackOptions.Verbose) Console.WriteLine(messageFunc());
		}

	}
}
