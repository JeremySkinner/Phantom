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
	using System.Collections;
	using System.IO;
	using Boo.Lang;

	/// <summary>
	/// Provides MSBuild integration
	/// </summary>
	public class msbuild : ExecutableTool<msbuild> {
		public msbuild() {
			toolPath = Path.Combine(Environment.GetEnvironmentVariable("windir"), "Microsoft.NET/Framework/v3.5/msbuild.exe");
			configuration = "debug";
			verbosity = "minimal";
			targets = new[] {"build"};
			properties = new Hash();
		}

		public string configuration { get; set; }
		public string[] targets { get; set; }
		public string verbosity { get; set; }
		public Hash properties { get; set; }
		public string file { get; set; }

		protected override void Execute() {
			if (string.IsNullOrEmpty(file)) {
				throw new InvalidOperationException("Please specify the 'path' property for calls to msbuild.");
			}

			string args = file + " /p:Configuration=" + configuration + " /t:" + string.Join(";", targets) + " /v:" + verbosity;

			foreach (DictionaryEntry entry in properties) {
				args += "/p:" + entry.Key + "=" + entry.Value;
			}

			Execute(args);
		}
	}
}