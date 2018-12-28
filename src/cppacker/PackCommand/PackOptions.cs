using CommandLine;
using System;

namespace cppacker.Packing
{

	[Verb("pack")]
	public class PackOptions : ProgramOptions
	{

		[Value(0)]
		public string ProjectFile { get; set; }

		[Option("name")]
		public string ProjectName { get; set; }

		[Option("version")]
		public string Version { get; set; }

		[Option("outputpath", HelpText="Specifies an outputpath to write files.")]
		public string OutputPath { get; set; }

		[Option("targetfile", HelpText="Default targetfile value when not specified in particular files. Default is lib.cs")]
		public string TargetFile { get; set; }
		

	}
}
