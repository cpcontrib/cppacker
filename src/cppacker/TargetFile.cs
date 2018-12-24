using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cppacker
{
	public class TargetFile
	{
		public TargetFile(string name)
		{
			this.Name = name;
			this.SourceDocs = new List<SrcDoc>();
			this.GlobalUsings = new List<String>();
		}

		public List<SrcDoc> SourceDocs { get; private set; }

		public List<string> GlobalUsings { get; set; }

		public string Name { get; private set; }


	}
}
