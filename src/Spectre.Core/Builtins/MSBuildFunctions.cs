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

namespace Spectre.Core.Builtins {
	using System.Runtime.CompilerServices;
	using Boo.Lang;

	[CompilerGlobalScope]
	public sealed class MSBuildFunctions {
		/// <summary>
		/// Executes msbuild
		/// </summary>
		/// <param name="file">The path to the file to build</param>
		public static void msbuild(string file) {
			msbuild(file, new Hash());
		}

		/// <summary>
		/// Executes msbuild
		/// </summary>
		/// <param name="file">The path to the file to build</param>
		/// <param name="options">Hash of options</param>
		public static void msbuild(string file, Hash options) {
			string frameworkVersion = options.ObtainAndRemove("frameworkVersion", "3.5");
			string configuration = options.ObtainAndRemove("configuration", "debug");
			string targets = options.ObtainAndRemove("targets", "build");

			string msbuildDir = UtilityFunctions.env("windir") + "\\microsoft.net\\framework\\v" + frameworkVersion + "\\msbuild.exe";
			string args = "/p:Configuration=" + configuration + " /t:" + targets;

			foreach (var key in options.Keys) {
				args += " /p:" + key + "=" + options[key];
			}

			UtilityFunctions.exec(msbuildDir, args);
		}
	}
}