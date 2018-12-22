using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cppacker
{
	public struct PackerDirective
	{
		public PackerDirective(string name, string options)
		{
			this.Name = name;
			this.Options = options ?? "";
		}

		public string Name;
		public string Options;
	}
	public class PackerDirectiveVisit
	{
		public PackerDirective PackerDirective;
		public int LineNumber;
		public string Name { get => PackerDirective.Name; }
		public string Options { get => PackerDirective.Options; }

	}
}
