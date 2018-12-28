using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cppacker
{

	/// <summary>
	/// Represents a Comment Directive Trivia (in Roslyn speak) that was translated into a Packer Directive
	/// </summary>
	public class PackerDirectiveNode
	{
		public string Name;
		public string Options;
		public int LineNumber;
	}
}
