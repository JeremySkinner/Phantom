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

namespace Phantom.Core {
	using System;
	using System.Collections.Generic;
	using Mono.Options;

	[Serializable]
	public class PhantomOptions {
		readonly List<string> targetNames = new List<string>();
		readonly OptionSet parser;

		public PhantomOptions() {
			File = "build.boo";

			parser = new OptionSet {
				{"f|file=", "Specifies the build file.", x => File = x.Trim() },
				{"h|help", "Show the help message.", v => Help = true},
				{"t|targets", "Shows all the targets in the specified build file.", x => ShowTargets = true},
				{"debugger", "Attach the debugger.", v => AttachDebugger = true},
				{"a|arg=", "Additional arguments.", v => AddArgument(v)},
				{"<>", "Targets", v => AddTarget(v)}
			};
		}

		public string File { get; set; }
		public bool Help { get; set; }
		public bool ShowTargets { get; set; }
		public bool AttachDebugger { get; set; }

		public IEnumerable<string> TargetNames {
			get {
				if (targetNames.Count == 0) {
					yield return "default";
				}
				else {
					foreach (var targetName in targetNames) {
						yield return targetName;
					}
				}
			}
		}

		public void AddTarget(string target) {
			targetNames.Add(target);
		}

		public void AddArgument(string value) {
			if (value.Contains("=")) {
				var bits = value.Split('=');
				Environment.SetEnvironmentVariable(bits[0], bits[1]);
			}
			else {
				Environment.SetEnvironmentVariable(value, String.Empty);
			}
		}

		public void PrintHelp() {
			Console.WriteLine("phantom [-f <filename>] [-t] [-h] [-a:<name>=<value>] targets");
			Console.WriteLine();
			parser.WriteOptionDescriptions(Console.Out);
		}

		public void Parse(string[] args) {
			parser.Parse(args);
		}
	}
}