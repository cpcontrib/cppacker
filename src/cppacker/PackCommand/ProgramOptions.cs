using CommandLine;
using System;

namespace cppacker
{
	public class ProgramOptions 
	{

		[Option("quiet")]
		public bool Quiet { get; set; }

		[Option('v',"verbose")]
		public bool Verbose { get; set; }

	}
}
