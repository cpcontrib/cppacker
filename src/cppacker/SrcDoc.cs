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
		public Document Document;
		public IEnumerable<PackerDirectiveNode> PackerDirectives;
		public SyntaxTree SyntaxTree;
	}
}
