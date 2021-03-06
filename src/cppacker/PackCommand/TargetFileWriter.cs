﻿using System;
using System.IO;

namespace cppacker.Packing
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
			try
			{
				string filepath = GetOutputFilePath(targetfile);

				using(FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
				{
					using(StreamWriter sw = new StreamWriter(fs))
					{
						Write(targetfile, sw);
					}
				}
			}
			catch(Exception ex)
			{
				throw new ApplicationException($"Failed to write target file '{targetfile.Name}'.", ex);
			}
		}

		public string GetOutputFilePath(TargetFile targetfile)
		{
			string filepath = Path.Combine(_outputPath, _baseName + targetfile.Name);
			return filepath;
		}

		public void Write(TargetFile targetFile, TextWriter writer)
		{
			writer.WriteLine("//packed");

			//write global usings
			foreach(var usingStatement in targetFile.GlobalUsings)
			{
				writer.WriteLine($"using {usingStatement};");
			}
			if(targetFile.GlobalUsings.Count > 0) writer.WriteLine();

			//write source docs
			foreach(var srcdoc in targetFile.SourceDocs)
			{
				writer.WriteLine($"//packed:{srcdoc.Document.Name}");

				srcdoc.SyntaxTree.GetText().Write(writer);

				writer.WriteLine($"//packed:{srcdoc.Document.Name}");
			}

		}

	}
}
