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

		public IEnumerable<PackerDirectiveVisit> ParseLines(IEnumerable<string> lines, int startingLineIndex = 0)
		{
			
			List<PackerDirectiveVisit> directives = new List<PackerDirectiveVisit>();

			int lineIndex = startingLineIndex;
			foreach(var line in lines)
			{
				Match m;
				if((m=packerDirectivesRegex.Match(line)).Success==true)
				{
					string[] groups = m.Groups[1].Value.Split(';');
					foreach(var g in groups)
					{
						string[] split = g.Split('=');
						var PackerDirective = new PackerDirective(split[0], (split.Length < 2 ? null : split[1]));
						var node = new PackerDirectiveVisit() {
							PackerDirective = PackerDirective,
							LineNumber = lineIndex + 1
						};

						directives.Add(node);
					}
				}

				startingLineIndex++;
			}

			return directives;
		}

	}
}
