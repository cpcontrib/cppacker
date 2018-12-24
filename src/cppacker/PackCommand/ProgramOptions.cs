using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
