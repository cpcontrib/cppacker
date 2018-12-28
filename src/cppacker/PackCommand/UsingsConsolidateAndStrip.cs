using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cppacker.Packing
{
	public class UsingsConsolidateAndStrip
	{

		public IEnumerable<UsingDirectiveSyntax> FindUsings(SrcDoc srcDoc)
		{
			//FindUsingsWalker w = new FindUsingsWalker();
			//w.Visit(srcdoc.SyntaxTree.GetRoot());
			//var usings = w.UsingStatements;
			//return usings;

			var compilationUnit = srcDoc.SyntaxTree.GetCompilationUnitRoot();

			return compilationUnit.Usings;
		}

		public CompilationUnitSyntax RemoveTopLevelUsings(SrcDoc srcDoc)
		{
			var emptylist = new SyntaxList<UsingDirectiveSyntax>();
			var newsyntax = srcDoc.SyntaxTree.GetCompilationUnitRoot().WithUsings(emptylist);

			return newsyntax;
		}

		private class RemoveUsingsRewriter : CSharpSyntaxRewriter
		{


			//public RemoveUsingsRewriter(SemanticModel semanticModel) => SemanticModel = semanticModel;
			public RemoveUsingsRewriter() { }

			public override SyntaxNode VisitUsingDirective(UsingDirectiveSyntax node)
			{
				return base.VisitUsingDirective(node);
			}
		}

		/* dont need this CompilationUnitRoot has Usings property that does this for us.
		 * private class FindUsingsWalker : CSharpSyntaxWalker
		{

			public List<UsingDirectiveSyntax> UsingStatements = new List<UsingDirectiveSyntax>();

			public override void VisitUsingStatement(UsingStatementSyntax node)
			{
				base.VisitUsingStatement(node);
			}
			public override void VisitUsingDirective(UsingDirectiveSyntax node)
			{
				UsingStatements.Add(node);
				base.VisitUsingDirective(node);
			}
		}*/

	}


}
