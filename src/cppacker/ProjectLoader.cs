using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cppacker
{
	public class ProjectLoader
	{
		public ProjectLoader(string projectFile)
		{
			this.ProjectFile = projectFile;
		}
		protected string ProjectFile;

		public IEnumerable<CSharpSyntaxTree> BuildSyntaxTrees()
		{
			var getfilenodes = GetFileNodes();
			throw new NotImplementedException();
		}

		IEnumerable<XElement> GetFileNodes()
		{
			XDocument xdoc;
			using(var sr = new StreamReader(this.ProjectFile))
			{
				xdoc = XDocument.Load(sr);

				throw new NotImplementedException();
			}
		}
	}
}
