using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using cppacker.Packing;
using cppacker;

namespace cppacker_Tests
{
	[TestFixture]
	public class UsingsRewriter_Tests
	{

		[Test]
		public void FindUsings()
		{
			//arrange
			string[] startingUsings = new string[] { "System", "System.Collections", "System.Text" };

			//act
			var srcdoc = new SrcDoc() { SyntaxTree = CSharpSyntaxTree.ParseText(helloworld1_cs) };
			UsingsConsolidateAndStrip usingsrewriter = new UsingsConsolidateAndStrip();
			var usings = usingsrewriter.FindUsings(srcdoc);

			//assert
			usings.Select(_ => _.Name.ToString()).ToArray()
				.Should().BeEquivalentTo(startingUsings);
		}

		[Test]
		public void RemoveUsings()
		{
			//arrange
			var srcDoc = new SrcDoc() { SyntaxTree = CSharpSyntaxTree.ParseText(helloworld1_cs) };

			//act
			UsingsConsolidateAndStrip usingsrewriter = new UsingsConsolidateAndStrip();
			var updatedRoot = usingsrewriter.RemoveTopLevelUsings(srcDoc);

			//assert
			updatedRoot.Usings.Count.Should().Be(0);
		}

		//[Test]
		//public void CombinedUsings()
		//{
		//	//arrange
		//	string[] expected_globalUsings = new string[] { "System", "System.Collections", "System.Linq", "System.Text" };

		//	var srcDoc1 = new SrcDoc() { SyntaxTree = CSharpSyntaxTree.ParseText(helloworld1_cs) };
		//	var srcDoc2 = new SrcDoc() { SyntaxTree = CSharpSyntaxTree.ParseText(helloworld2_cs) };

		//	//act
		//	UsingsConsolidateAndStrip usingsrewriter = new UsingsConsolidateAndStrip();
		//	var globalUsings = usingsrewriter.RemoveTopLevelUsings(srcDoc1);

		//	//assert
		//	var actual = globalUsings.Usings.Select(_ => _.Name.ToString());

		//	actual.Should().BeEquivalentTo(expected_globalUsings);
		//}


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
