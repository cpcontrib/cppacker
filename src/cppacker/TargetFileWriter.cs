using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace cppacker
{
	public class TargetFileWriter
	{
		public TargetFileWriter(string outputPath, string basename)
		{
			this._outputPath = outputPath;
			this._baseName = basename;
		}

		private string _outputPath;
		private string _baseName;

		public void Write(TargetFile targetfile)
		{
			string filepath = Path.Combine(_outputPath, _baseName + targetfile.Name);
			

			using(FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
			{
				using(StreamWriter sw = new StreamWriter(fs))
				{
					Write(targetfile, sw);
				}
			}
		}

		public void Write(TargetFile targetFile, TextWriter writer)
		{
			writer.WriteLine("//packed");

			//write global usings
			foreach(var usingStatement in targetFile.GlobalUsings)
			{
				writer.WriteLine(usingStatement.ToFullString());
			}

			//write source docs
			foreach(var srcdoc in targetFile.SourceDocs)
			{
				writer.WriteLine($"//packed:{srcdoc.Document.Name}");

				srcdoc.Document.GetTextAsync().Result.Write(writer);

				writer.WriteLine($"//packed:{srcdoc.Document.Name}");
			}

		}

	}
}
