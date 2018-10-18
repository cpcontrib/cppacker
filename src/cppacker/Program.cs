using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using PackOptions = cppacker.Pack.PackOptions;
using PackCommand = cppacker.Pack.PackCommand;
using CommandLine;

namespace cppacker
{
	class Program
	{
		static void Main(string[] args)
		{
			var PackOptions = new PackOptions() {
				ProjectFile = args[1]
			};

			var validation = new PackCommand().ValidateOptions(PackOptions); 
			if(validation.IsValid == false)
			{
				Console.WriteLine("some options not valid:");
				foreach(var message in validation.GetMessages())
					Console.WriteLine(message);
			}
			else
			{

				PackCommand packer = new PackCommand(PackOptions);
				try
				{
					int exitcode = packer.Execute();
					Exit(exitcode);
				}
				catch(Exception ex)
				{
					Console.WriteLine(ex);
					Exit(1);
				}

				
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
