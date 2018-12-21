using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSG;

namespace cppacker.Pack
{
	public class PackDocumentFactory
	{

		public Document Document;

		public Promise GetSyntaxTree;
		public Promise GetPackerDirectives;

		public RSG.Promise<PackDocument> Create()
		{
			return new Promise<PackDocument>((resolve, reject) => {

				Promise<object>.All((IPromise<object>)GetSyntaxTree, (IPromise<object>)GetPackerDirectives)
				.Then(results => {
					resolve(new PackDocument() {
						Document = Document,
						SyntaxTree = (SyntaxTree)results.ElementAt(0),
						PackerDirectives = (IEnumerable<PackerDirective>)results.ElementAt(1)
					});
				});

			});
		}

	}
}
