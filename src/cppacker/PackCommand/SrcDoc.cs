using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace cppacker.Packing
{
	public class SrcDoc
	{
		public Document Document;
		public IEnumerable<PackerDirectiveNode> PackerDirectives;
		public SyntaxTree SyntaxTree;
	}
}
