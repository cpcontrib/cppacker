using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cppacker.PackCommand
{
	public struct PackerDirective
	{
		public PackerDirective(string name, string options)
		{
			this.Name = name;
			this.Options = options == null ? "" : options;
		}

		public string Name;
		public string Options;
	}
}
