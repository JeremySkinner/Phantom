#region License

// Copyright Jeremy Skinner (http://www.jeremyskinner.co.uk) and Contributors
// 
// Licensed under the Microsoft Public License. You may
// obtain a copy of the license at:
// 
// http://www.microsoft.com/opensource/licenses.mspx
// 
// By using this source code in any fashion, you are agreeing
// to be bound by the terms of the Microsoft Public License.
// 
// You must not remove this notice, or any other, from this software.

#endregion

namespace Phantom {
	using System;
	using System.Linq;
	using Core;

	internal class Program {
		static void Main(string[] args) {
			var program = new Program();
			program.Execute(args);
		}

		void Execute(string[] args) {
			try {
				WriteHeader();

				var options = new PhantomOptions();

				try {
					options.Parse(args);
				}
				catch (Exception exception) {
					Console.WriteLine(exception.Message);
					options.PrintHelp(Console.Out);
					return;
				}

				if (options.Help) {
					options.PrintHelp(Console.Out);
					return;
				}

				var runner = BuildRunner.Create();

				if (options.ShowTargets) {
					runner.OutputTargets(options);
					return;
				}

				PrintSelectedTargets(options);
				runner.Execute(options);
			}
			catch (Exception exception) {
				Environment.ExitCode = 1;
				Console.WriteLine(exception);
			}
		}

		void PrintSelectedTargets(PhantomOptions options) {
			string targets = string.Join(", ", options.TargetNames.ToArray());
			Console.WriteLine("Targets specified: {0}", targets);
			Console.WriteLine();
			Console.WriteLine();
		}

		void WriteHeader() {
			string version = typeof (Program).Assembly.GetName().Version.ToString();
			Console.WriteLine("Phantom v{0}", version);
			Console.WriteLine(string.Format("Copyright (c) 2009-{0} Jeremy Skinner and Contributors", DateTime.Today.Year));
			Console.WriteLine("http://github.com/JeremySkinner/Phantom");
			Console.WriteLine();
			Console.WriteLine();
		}
	}
}