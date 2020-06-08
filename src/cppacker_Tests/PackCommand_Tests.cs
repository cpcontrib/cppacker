using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;
using cppacker.Packing;
using Microsoft.CodeAnalysis.CSharp;
using FluentAssertions;

namespace cppacker_Tests
{

	[TestFixture]
	public class PackCommand_Tests
	{

		[Test]
		public void Check_ConsolidatedUsings()
		{
			//arrange
			string[] expected_ConsolidatedUsings = new string[] { "System", "System.Collections", "System.Linq", "System.Text" };

			var srcdocList = new List<SrcDoc>();
			srcdocList.Add(new SrcDoc() { SyntaxTree = CSharpSyntaxTree.ParseText(helloworld1_cs) });
			srcdocList.Add(new SrcDoc() { SyntaxTree = CSharpSyntaxTree.ParseText(helloworld2_cs) });

			//act
			var packcmd = new PackCommand(new PackOptions());

			var targetfiles = packcmd.GenerateTargetFilesList(srcdocList);

			packcmd.ConsolidateAndStripGlobalUsings(targetfiles);

			targetfiles.Should().HaveCount(1);
			targetfiles.ElementAt(0).GlobalUsings.Select(_=>_.ToString())
				.Should().BeEquivalentTo(expected_ConsolidatedUsings);
		}

		[Test]
		public void Check_GeneratePackedLibraryFiles()
		{
			//arrange
			string[] expected_ConsolidatedUsings = new string[] { "System", "System.Collections", "System.Linq", "System.Text" };

			var srcdocList = new List<SrcDoc>();
			srcdocList.Add(new SrcDoc() { SyntaxTree = CSharpSyntaxTree.ParseText(helloworld1_cs) });
			srcdocList.Add(new SrcDoc() { SyntaxTree = CSharpSyntaxTree.ParseText(helloworld2_cs) });

			//act
			var packcmd = new PackCommand(new PackOptions());

			var targetfiles = packcmd.GenerateTargetFilesList(srcdocList);

			packcmd.GeneratePackedLibraryFiles(targetfiles);

			targetfiles.Should().HaveCount(1);
			targetfiles.ElementAt(0).GlobalUsings.Select(_ => _.ToString())
				.Should().BeEquivalentTo(expected_ConsolidatedUsings);
		}

		string helloworld1_cs = @"
using System;
using System.Collections;
using System.Text;
 
namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Hello, World!"");
        }
    }
}";
		string helloworld2_cs = @"
using System;
using System.Linq;
using System.Text;
 
namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Hello, World!"");
        }
    }
}";
	}
}
