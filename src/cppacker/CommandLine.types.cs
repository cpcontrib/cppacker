using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLine
{
	public interface ICommand
	{
		int Execute();
		OptionsValidation ValidateOptions(ICommandOptions);
	}
	public interface ICommandOptions
	{
	}
	public class OptionsValidation
	{
		List<string> messages = new List<string>();

		public bool IsValid { get { return this._IsValid; } }
		private bool _IsValid;

		public IEnumerable<string> GetMessages()
		{
			return messages;
		}

		public void AddMessage(string optionName, string message, bool isValid = true)
		{
			this.messages.Add(optionName + ": " + message);
			if(isValid == false) this._IsValid = false;
		}


	}
}
