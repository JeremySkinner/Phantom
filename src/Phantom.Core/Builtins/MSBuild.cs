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
			toolPath = BuildMsbuildPath("4.0");
			configuration = "debug";
			verbosity = "minimal";
			targets = new[] {"build"};
			properties = new Hash();
		}

		private string BuildMsbuildPath(string version) {
			if(version == "4.0" || version == "4") {
				version = "4.0.30319";
			}
			
			string windir = Environment.GetEnvironmentVariable("windir");
			if (windir != null) {
				return Path.Combine(windir, "Microsoft.NET/Framework/v" + version + "/msbuild.exe");
			}
			
			return "/usr/bin/xbuild";
		}

		public string configuration { get; set; }
		public string[] targets { get; set; }
		public string verbosity { get; set; }
		public Hash properties { get; set; }
		public string file { get; set; }
		
		string _version;

		public string version {
			get { return _version; }
			set {
				_version = value;
				toolPath = BuildMsbuildPath(value);
			}
		}

		protected override void Execute() {
			if (string.IsNullOrEmpty(file)) {
				throw new InvalidOperationException("Please specify the 'file' property for calls to msbuild.");
			}

			string args = file + " /p:Configuration=" + configuration + " /t:" + string.Join(";", targets) + " /v:" + verbosity;

			foreach (DictionaryEntry entry in properties) {
				args += " /p:" + entry.Key + "=" + entry.Value;
			}

			Execute(args);
		}
	}
}