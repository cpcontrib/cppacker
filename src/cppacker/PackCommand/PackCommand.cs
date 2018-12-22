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

				var factory = new PackDocumentFactory();

				var packerDirectives = GetPackerDirectives(document);

				if(packerDirectives.Count() > 0 && packerDirectives.Any(_ => _.Name == "exclude") == true)
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

		public IEnumerable<PackerDirectiveVisit> GetPackerDirectives(Document document)
		{
			System.Diagnostics.Debugger.Break();
			PackerDirectivesParser p = new PackerDirectivesParser();
			return p.ParseLines(new string[] { "" }); 
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
