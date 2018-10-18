using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cppacker
{
	class Program
	{
		static void Main(string[] args)
		{
			ProgramOptions ProgramOptions = new ProgramOptions() {
				ProjectFile = @"C:\Projects\cpcontrib\core\src\cpcontrib.core.csproj"
			};

			if(File.Exists(ProgramOptions.ProjectFile) == false)
			{
				Console.WriteLine("projectfile doesnt exist");
			}
			else
			{
				ProjectLoader projectLoader = new ProjectLoader(ProgramOptions.ProjectFile);

				var syntaxtrees = projectLoader.BuildSyntaxTrees();
			}
		}


	}
}
