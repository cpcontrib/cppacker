using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace cppacker.Packing
{
	public class PackOptionsValidator : IOptionsValidator<PackOptions>
	{
		public PackOptionsValidator(object FileSystem)
		{
			//FileSystem should be IFileSystem wrapper
		}

		public OptionsValidation Validate(PackOptions PackOptions)
		{
			var validation = new CommandLine.OptionsValidation();

			if(System.IO.File.Exists(PackOptions.ProjectFile) == false)
			{
				validation.AddMessage("ProjectFile", $"The file specified '{PackOptions.ProjectFile}' was not found.");
			}

			return validation;
		}


	}
}
