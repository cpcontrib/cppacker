﻿using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using PackOptions = cppacker.Pack.PackOptions;
using PackCommand = cppacker.Pack.PackCommand;
using CommandLine;

namespace cppacker
{
	class Program
	{
		static void Main(string[] args)
		{
			var PackOptions = new PackOptions() {
				ProjectFile = args[1]
			};

			var validation = new PackCommand().ValidateOptions(PackOptions); 
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
					int exitcode = packer.Execute();
					Environment.Exit(exitcode);
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
