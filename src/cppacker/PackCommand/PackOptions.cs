using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cppacker.Pack
{
	public class PackOptions : CommandLine.ICommandOptions
	{
		public string ProjectFile { get; set; }
	}
}
