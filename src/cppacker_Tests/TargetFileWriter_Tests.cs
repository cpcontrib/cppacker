using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using FluentAssertions;
using cppacker.Packing;
using System.IO;

namespace cppacker_Tests
{
	[TestFixture]
	public class TargetFileWriter_Tests
	{

		const string OutputPath = "R:\\temp";
		const string BaseName = "_TestProj.";

		[Test]
		public void Check_GetOutputFilePath()
		{
			string targetfile_Name = "lib";
			string expected_OutputPath = Path.Combine(OutputPath, BaseName) + targetfile_Name;

			//arrange
			var targetfile = new TargetFile(targetfile_Name);

			//act
			var targetfilewriter = new TargetFileWriter(OutputPath, BaseName);
			var actual = targetfilewriter.GetOutputFilePath(targetfile);

			actual.Should().Be(expected_OutputPath);
		}

		[Test]
		public void Check_Write_TextWriter()
		{
			string targetfile_Name = "lib";
			string expected_OutputPath = Path.Combine(OutputPath, BaseName) + targetfile_Name;

			//arrange
			var targetfile = new TargetFile(targetfile_Name);
			var sw = new StringWriter();

			//act
			var targetfilewriter = new TargetFileWriter(OutputPath, BaseName);
			targetfilewriter.Write(targetfile, sw);

			sw.ToString().Should().NotBeNullOrEmpty();
		}

		string filecontent1 = @"
using System;

namespace XYZ
{
	public class ABC { }
}
";
		string filecontent2 = @"
using System;

namespace XYZ
{
	public class ABC { }
}
";

	}
}
