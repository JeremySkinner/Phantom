﻿#region License

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
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;

	public class mstest : TestRunner<mstest> {
		readonly StringBuilder args;

		public mstest() {
			args = new StringBuilder();
			toolPath = FindToolPath();
		}

		public string[] options { get; set; }
		public string[] tests { get; set; }

		protected override void Execute() {
			foreach (var container in assemblies) {
				args.AppendFormat(" /testcontainer:\"{0}\"", container);
			}

			if (tests != null && tests.Length != 0) {
				foreach (var test in tests) {
					args.AppendFormat(" /test:{0}", test);
				}
			}

			if (options != null && options.Length != 0) {
				args.Append(string.Join(" ", options));
			}

			Execute(args.ToString());
		}

		private string FindToolPath() {
			var defaultPaths = new List<string> {
				@"C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\mstest.exe", 
				@"C:\Program Files\Microsoft Visual Studio 10.0\Common7\IDE\mstest.exe", 
				@"C:\Program Files (x86)\Microsoft Visual Studio 9.0\Common7\IDE\mstest.exe", 
				@"C:\Program Files\Microsoft Visual Studio 9.0\Common7\IDE\mstest.exe"
			};

			return defaultPaths.FirstOrDefault(File.Exists);
		}
	}
}