using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cppacker.Pack
{
	public class PackOptions : ProgramOptions
	{
		public string ProjectFile { get; set; }
		public string ProjectName { get; set; }
		public string Version { get; set; }

		public string OutputPath { get; set; }
	}
}
