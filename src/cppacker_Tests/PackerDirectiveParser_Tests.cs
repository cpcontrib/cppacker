using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using cppacker.Parsing;
using cppacker;

namespace cppacker_Tests
{
	[TestFixture]
	public class PackerDirectiveParser_Tests
	{
		[Test]
		[TestCase("//!packer:targetFile=filenamespec.cs", "targetFile", "filenamespec.cs")]
		[TestCase("//!packer:region=ASDF ASDF", "region", "ASDF ASDF")]
		[TestCase("//!packer:endregion", "endregion", "")]
		public void TestMethod(string rawtext, string directiveName, string directiveOptions)
		{
			var parser = CreatePackerDirectivesParser();

			var parsed = parser.ParseLines(new string[] { rawtext })
				.ToArray();

			Assert.That(parsed[0], Is.TypeOf<PackerDirectiveNode>());

			var directive = parsed[0];

			Assert.That(directive.Name, Is.EqualTo(directiveName));
			Assert.That(directive.Options, Is.EqualTo(directiveOptions));
		}

		[Test]
		[TestCase("//!packer:targetFile=XXTARGETFILEXX;sortOrder=XXSORTORDERXX;asdf=XXASDFXX", "targetFile,sortOrder,asdf")]
		public void DelimitedOptions(string rawtext, string directiveNamesString)
		{
			string[] expectedNames = directiveNamesString.Split(',');
			string[] expectedOptions = expectedNames.Select(item => "XX" + item.ToUpper() + "XX").ToArray();

			var parser = CreatePackerDirectivesParser();

			var parsed = parser.ParseLines(new string[] { rawtext })
				.ToArray();

			for(int index = 0; index < expectedNames.Length; index++)
			{
				Assert.That(parsed[index].Name, Is.EqualTo(expectedNames[index]));
				Assert.That(parsed[index].Options, Is.EqualTo(expectedOptions[index]));
			}
	
		}

		private PackerDirectivesParser CreatePackerDirectivesParser()
		{
			return new PackerDirectivesParser();
		}

	}
}
