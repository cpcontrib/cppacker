using System;
using System.Collections.Generic;

namespace cppacker.Packing
{
	public class TargetFile
	{
		public TargetFile(string name)
		{
			this.Name = name;
			this.SourceDocs = new List<SrcDoc>();
			this.GlobalUsings = new List<string>();
		}

		public List<SrcDoc> SourceDocs { get; private set; }

		public List<string> GlobalUsings { get; set; }

		public string Name { get; private set; }


	}
}
