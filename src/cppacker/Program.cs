using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using cppacker.PackCommand;

namespace cppacker
{
	class Program
	{
		static void Main(string[] args)
		{
			var PackOptions = new PackOptions() {
				ProjectFile = @"C:\Projects\cpcontrib\core\src\cpcontrib.core\cpcontrib.core.csproj"
			};

			var validation = PackCommand.Validate(PackOptions); 
			if(validation.IsValid == false)
			{
				Console.WriteLine("some options not valid:");
				foreach(var message in validation.GetMessages())
					Console.WriteLine(message);
			}
			else
			{

				PackCommand packer = new PackCommand(PackOptions);
				try
				{
					return packer.Execute();
				}
				catch(Exception ex)
				{
					Console.WriteLine(ex);
					Environment.Exit(1);
				}

			}
		}


	}
}
