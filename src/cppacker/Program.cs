using System;
using System.Collections.Generic;
using System.IO;

using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

using CommandLine;

namespace cppacker
{
	using cppacker.Packing;

	/// <summary>
	/// Created because of ParseArguments bug that doesnt deal with a single verb
	/// </summary>
	[Verb("dummy", Hidden = true)] public class DummyOptions { }

	[ExcludeFromCodeCoverage]
	class Program
	{
		static void Main(string[] args)
		{

			try
			{
				int exitcode = CommandLine.Parser.Default
					.ParseArguments<PackOptions, DummyOptions>(args)
					.MapResult(
					(PackOptions opts) => {
						var val = new PackOptionsValidator(null).Validate(opts).WriteWhenNotValid(Console.Error);
						if(val.IsValid == false) return val.NonZeroExit();

						var cmd = new PackCommand(opts);
						return cmd.Execute();
					},
					(DummyOptions opts) => 255,
					(parserErrors) =>
					255
					);

				Exit(exitcode);
			}
			catch(Exception ex)
			{
				Console.Error.WriteLine(ex.ToString());
				Exit(255);
			}

		}
		static void Exit(int exitcode, int waitSeconds = 10, bool quiet = false)
		{
			if(System.Diagnostics.Debugger.IsAttached)
			{
				if(quiet == false)
				{
					if(exitcode > 0) Console.WriteLine("Exit code: {0}", exitcode);
					Console.WriteLine("Debugger attached: pausing for {0} seconds", waitSeconds);
					Task.Factory.StartNew(() => WaitForKey(waitSeconds)).Wait(TimeSpan.FromSeconds(waitSeconds));
				}
				else
				{
					Task.Factory.StartNew(() => Console.ReadKey()).Wait(TimeSpan.FromSeconds(waitSeconds));
				}
			}
		}
		static void WaitForKey(int waitSeconds = 10, string message = null)
		{
			message = message ?? "Debugger attached: pausing for ";

			var original = DateTime.Now;
			var newTime = original;

			var remainingWaitTime = waitSeconds;
			var lastWaitTime = waitSeconds.ToString();
			var keyRead = false;
			Console.Write(message + waitSeconds);
			do
			{
				keyRead = Console.KeyAvailable;
				if(!keyRead)
				{
					newTime = DateTime.Now;
					remainingWaitTime = waitSeconds - (int)(newTime - original).TotalSeconds;
					var newWaitTime = remainingWaitTime.ToString();
					if(newWaitTime != lastWaitTime)
					{
						var backSpaces = new string('\b', lastWaitTime.Length);
						var spaces = new string(' ', lastWaitTime.Length);
						Console.Write(backSpaces + spaces + backSpaces);
						lastWaitTime = newWaitTime;
						Console.Write(lastWaitTime);
						System.Threading.Thread.Sleep(100);
					}
				}
			} while(remainingWaitTime > 0 && !keyRead);
		}


	}
}
