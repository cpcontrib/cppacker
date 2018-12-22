using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cppacker
{
	public class ProgramOptions : CommandLine.ICommandOptions
	{
		public bool Quiet { get; set; }
		public bool Verbose { get; set; }

	}
}
