using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace cppacker
{
	public class SrcDoc
	{
		internal Document Document;
		internal IEnumerable<PackerDirectiveNode> PackerDirectives;
		internal SyntaxTree SyntaxTree;
	}
}
