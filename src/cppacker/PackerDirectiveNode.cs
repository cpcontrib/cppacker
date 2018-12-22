using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cppacker
{

	public class PackerDirectiveNode
	{
		public string Name;
		public string Options;
		public int LineNumber;
		public Document SrcDoc;
	}
}
