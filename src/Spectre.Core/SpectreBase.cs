#region License

// Copyright 2009 Jeremy Skinner (http://www.jeremyskinner.co.uk)
//  
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
//  
// http://www.apache.org/licenses/LICENSE-2.0 
//  
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.
// 
// The latest version of this file can be found at http://github.com/JeremySkinner/Spectre

#endregion

namespace Spectre.Core {
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using Boo.Lang;

	public abstract class SpectreBase {
		readonly ScriptModel model = new ScriptModel();

		public void target(string name, Action block) {
			model.AddTarget(name, null, block);
		}

		public void target(string name, string[] dependencies, Action block) {
			model.AddTarget(name, dependencies, block);
		}

		public abstract void Execute();

		public ScriptModel Model {
			get { return model; }
		}

		public void desc(string description) {
			model.SetCurrentDescription(description);
		}

		public void exec(string command, string args) {
			var psi = new ProcessStartInfo(command, args);
			psi.UseShellExecute = false;
			psi.RedirectStandardError = true;
			var process = Process.Start(psi);
			process.WaitForExit();
		}

		public void msbuild(string file) {
			msbuild(file, new Hash());
		}

		public void msbuild(string file, Boo.Lang.Hash options) {
			string frameworkVersion = options.ObtainAndRemove("frameworkVersion", "3.5");
			string configuration = options.ObtainAndRemove("configuration", "debug");
			string targets = options.ObtainAndRemove("targets", "build");

			string msbuildDir = env("windir") + "\\microsoft.net\\framework\\v" + frameworkVersion + "\\msbuild.exe";
			string args = "/p:Configuration=" + configuration + " /t:" + targets;

			foreach(var key in options.Keys) {
				args += " /p:" + key + "=" + options[key];
			}
			exec(msbuildDir, args);
		}

		public string env(string variableName) {
			return System.Environment.GetEnvironmentVariable(variableName);
		}

		public void nunit(string assembly) {
			nunit(new[] { assembly });
		}

		public void nunit(string[] assemblyPaths) {
			nunit(assemblyPaths, new Hash());
		}

		public void nunit(string[] assemblyPaths, Hash options) {
			string path = options.ObtainAndRemove("path", "lib\\nunit\\nunit-console.exe");
			foreach(var assembly in assemblyPaths) {
				exec(path, "\"" + assembly + "\"");
			}
		}
	}
}