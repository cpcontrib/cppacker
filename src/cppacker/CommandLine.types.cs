using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;


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

	[ExcludeFromCodeCoverage]
	public class OptionsValidation
	{
		List<Tuple<bool,string>> messages = new List<Tuple<bool, string>>();

		public bool IsValid { get { return this._IsValid; } }
		private bool _IsValid = true;

		public IEnumerable<string> GetMessages()
		{
			return messages.Where(_=>_.Item1 == true).Select(_=>_.Item2);
		}

		public OptionsValidation AddMessage(string optionName, string message, bool isValid = true)
		{
			this.messages.Add(new Tuple<bool,string>(isValid, optionName + ": " + message));
			if(isValid == false) this._IsValid = false;
			return this;
		}

		/// <summary>
		/// Writes any option validation messages to given TextWriter (usually Console.Error/stderr).
		/// Returns true if any error messages written out.
		/// </summary>
		/// <param name="writer"></param>
		/// <returns></returns>
		public OptionsValidation WriteWhenNotValid(System.IO.TextWriter writer)
		{
			if(IsValid == true) return this;

			var messages = GetMessages();

			if(messages.Count() > 0)
			{
				foreach(var message in messages)
				{
					writer.WriteLine(message);
				}
			}

			return this;
		}

		public int NonZeroExit()
		{
			if(this.IsValid == false)
				return 255;
			else
				return 0;
		}
	}
}
