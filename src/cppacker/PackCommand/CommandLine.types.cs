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
	}

	/// <summary>
	/// Interface for an command that can do options validation.
	/// </summary>
	public interface ICommandOptionsValidator
	{
		OptionsValidation Validate();
		bool IsValid();
	}

	/// <summary>
	/// Interface for an options validator.  Normally used for a separate class that validates options.  If your Command class validates directly, use <see cref="ICommandOptionsValidator"/>
	/// </summary>
	/// <typeparam name="Toptions"></typeparam>
	public interface IOptionsValidator<Toptions>
	{
		OptionsValidation Validate(Toptions options);
	}

	public class OptionsValidation
	{
		List<string> messages = new List<string>();

		public bool IsValid { get { return this._IsValid; } }
		private bool _IsValid = true;

		public IEnumerable<string> GetMessages()
		{
			return messages;
		}

		public void AddMessage(string optionName, string message, bool isValid = true)
		{
			this.messages.Add(optionName + ": " + message);
			if(isValid == false) this._IsValid = false;
		}

		/// <summary>
		/// Writes any option validation messages to given TextWriter (usually Console.Error/stderr).
		/// Returns true if any error messages written out.
		/// </summary>
		/// <param name="writer"></param>
		/// <returns></returns>
		public bool WriteMessages(System.IO.TextWriter writer)
		{
			var messages = GetMessages();

			if(messages.Count() == 0)
				return false;

			foreach(var message in messages)
			{
				writer.WriteLine(message);
			}

			return true;
		}

	}
}
