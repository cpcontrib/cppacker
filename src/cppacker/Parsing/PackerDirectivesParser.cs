
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace cppacker.Parsing
{
	public class PackerDirectivesParser
	{

		Regex packerDirectivesRegex = new Regex(@"//!packer:(.*)");

		public IEnumerable<PackerDirectiveNode> ParseLines(IEnumerable<string> lines, int startingLineIndex = 0)
		{
			
			List<PackerDirectiveNode> directives = new List<PackerDirectiveNode>();

			int lineIndex = startingLineIndex;
			foreach(var line in lines)
			{
				_ParseLine(line, directives, lineIndex);

				startingLineIndex++;
			}

			return directives;
		}


		private void _ParseLine(string line, List<PackerDirectiveNode> directives, int lineIndex)
		{
			Match m;
			if((m = packerDirectivesRegex.Match(line)).Success == true)
			{
				string[] groups = m.Groups[1].Value.Split(';');
				foreach(var g in groups)
				{
					string[] split = g.Split('=');

					var node = new PackerDirectiveNode() {
						Name = split[0],
						Options = (split.Length < 2 ? "" : split[1].TrimEnd()),
						LineNumber = lineIndex + 1
					};

					directives.Add(node);
				}
			}
		}
	}

}
