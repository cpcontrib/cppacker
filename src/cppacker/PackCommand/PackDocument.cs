using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace cppacker.Pack
{
	public struct PackDocument
	{
		internal Document Document;
		internal IEnumerable<PackerDirective> PackerDirectives;
	}
}
