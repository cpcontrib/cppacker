using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using cppacker.Parsing;

namespace cppacker_Tests
{
	[TestFixture]
	public class PackerDirectiveParser_Tests
	{
		[Test]
		[TestCase("//!packer:targetFile=filenamespec.cs", "targetFile", "filenamespec.cs")]
		[TestCase("//!packer:region=ASDF ASDF", "region", "ASDF ASDF")]
		[TestCase("//!packer:endregion", "endregion", null)]
		public void TestMethod(string rawtext, string directiveName, string directiveOptions)
		{
			var parser = CreatePackerParser();

			var parsed = parser.ParseLines(new string[] { rawtext })
				.ToArray();

			Assert.That(parsed[0], Is.TypeOf<ParserDirective>());



			Assert.That(directive.Name, Is.EqualTo(directiveName));
			Assert.That(directive.Options, Is.EqualTo(directiveOptions));
		}

		[Test]
		[TestCase("//!packer:targetFile=filenamespec.cs;sortOrder=999;asdf=ttt", "targetFile,sortOrder,asdf")]
		public void DelimitedOptions(string rawtext, string directiveNamesString)
		{
			var parser = CreatePackerParser();

			var parsed = parser.ParseLines(new string[] { rawtext })
				.ToArray();

			Assert.That(parsed[0].Type, Is.TypeOf<ParserDirectives>());

			var directives = parsed

			Assert.That(directive.Name, Is.EqualTo(directiveName));
			Assert.That(directive.Options, Is.EqualTo(directiveOptions));
		}

		private PackerDirectiveParser CreatePackerDirectiveParser()
		{
			return new PackerDirectiveParser();
		}

	}
}
