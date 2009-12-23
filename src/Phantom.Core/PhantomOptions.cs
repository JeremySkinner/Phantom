#region License

// Copyright Jeremy Skinner (http://www.jeremyskinner.co.uk)
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
// 
// The latest version of this file can be found at http://github.com/JeremySkinner/Phantom

#endregion

namespace Phantom.Core {
	using System;
	using System.Collections.Generic;
	using Boo.Lang.Useful.CommandLine;

	[Serializable]
	public class PhantomOptions : AbstractCommandLine {
		readonly List<string> targetNames = new List<string>();

		[Option("Specifies the build file", LongForm = "file", ShortForm = "f")]
		public string File = "build.boo";

		[Option("Prints the help message", LongForm = "help", ShortForm = "h")]
		public bool Help;

		[Option("Shows all the targets in the specified build file", LongForm = "targets", ShortForm = "t")]
		public bool ShowTargets;

        // Useful for debugging of scripts or Phantom itself
        [Option("Attaches debugger before build starts", LongForm = "debugger")]
        public bool AttachDebugger;

		[Argument]
		public void AddTarget(string targetName) {
			targetNames.Add(targetName);
		}

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

		[Option("Additional arguments", LongForm = "arg", ShortForm = "a", MaxOccurs = 50)]
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
			PrintOptions();
		}
	}
}