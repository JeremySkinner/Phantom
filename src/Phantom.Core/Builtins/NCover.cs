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

namespace Phantom.Core.Builtins {
	using System;
	using System.IO;

	/// <summary>
	/// NCover integration
	/// </summary>
	public class ncover : ExecutableTool<ncover> {
		public ncover() {
			reportDirectory = ".";
			workingDirectory = ".";
			applicationAssemblies = new string[0];
			args = new string[0];
		}

		public string excludeAttributes { get; set; }
		public string program { get; set; }
		public string testAssembly { get; set; }
		public string reportDirectory { get; set; }
		public string workingDirectory { get; set; }
		public string[] applicationAssemblies { get; set; }
		public string[] args { get; set; }

		protected override void Execute() {
			if (string.IsNullOrEmpty(program)) {
				throw new InvalidOperationException("The 'program' property must be specified.");
			}

			if (string.IsNullOrEmpty(testAssembly)) {
				throw new InvalidOperationException("The 'assembly' property must be specified.");
			}

			if (applicationAssemblies == null || applicationAssemblies.Length == 0) {
				throw new InvalidOperationException("Please specify the 'applicationAssemblies' property as an array of strings.");
			}

			IOFunctions.mkdir(reportDirectory);

			string assembliesJoined = applicationAssemblies.JoinWith(";");
			string additionalArgs = args.JoinWith(" ");

			var testAssemblyFile = new FileInfo(testAssembly);
			var reportDir = new DirectoryInfo(reportDirectory);

			string xmlPath = Path.Combine(reportDir.FullName, testAssemblyFile.Name + ".Coverage.xml");
			string logPath = Path.Combine(reportDir.FullName, testAssemblyFile.Name + ".Coverage.log");

			string applicationArgs = string.Format("{0} {1} {2} //a {3} //w {4} //x {5} //l {6} //v",
			                                       program, testAssembly, additionalArgs, assembliesJoined, workingDirectory,
			                                       xmlPath, logPath);

			if (! string.IsNullOrEmpty(excludeAttributes)) {
				applicationArgs += string.Format(" //ea {0}", excludeAttributes);
			}

			Execute(applicationArgs);
		}
	}
}