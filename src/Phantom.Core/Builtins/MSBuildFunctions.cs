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

namespace Phantom.Core.Builtins {
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
			string verbosity = options.ObtainAndRemove("verbosity", "minimal");

			string msbuildDir = UtilityFunctions.env("windir") + "\\microsoft.net\\framework\\v" + frameworkVersion +
			                    "\\msbuild.exe";
			string args = file + " /p:Configuration=" + configuration + " /t:" + targets + " /v:" + verbosity;

			foreach (var key in options.Keys) {
				args += " /p:" + key + "=" + options[key];
			}

			IOFunctions.exec(msbuildDir, args);
		}
	}
}