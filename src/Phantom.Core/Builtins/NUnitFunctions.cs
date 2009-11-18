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
	using System;
	using System.Runtime.CompilerServices;
	using Boo.Lang;

	[CompilerGlobalScope]
	public sealed class NUnitFunctions {
		/// <summary>
		/// Executes nunit against the specified assembly. Assumes nunit is located at lib\nunit\nunit-console.exe
		/// </summary>
		/// <param name="assembly"></param>
		public static void nunit(string assembly) {
			nunit(new[] {assembly});
		}

		/// <summary>
		/// Executes nunit against the specified assemblies. Assumes nunit is located at lib\nunit\nunit-console.exe
		/// </summary>
		/// <param name="assemblyPaths"></param>
		public static void nunit(string[] assemblyPaths) {
			nunit(assemblyPaths, new Hash());
		}

		/// <summary>
		/// Executes nunit against the specified assemblies.
		/// </summary>
		/// <param name="assemblyPaths">The assemblies</param>
		/// <param name="options">Additional options: path (path to unit), [include (categories to run) | exclude (categories to not run)]</param>
		public static void nunit(string[] assemblyPaths, Hash options) {
			string path = options.ObtainAndRemove("path", "lib\\nunit\\nunit-console.exe");
			string include = options.ObtainAndRemove("include", (string) null);
			string exclude = options.ObtainAndRemove("exclude", (string) null);
			bool enableTeamCity = options.ObtainAndRemove("enableTeamCity", false);
			string teamCityArgs = options.ObtainAndRemove("teamCityArgs", "v2.0 x86 NUnit-2.4.6");

			var args = new List<string>();
			
			if(enableTeamCity) {
				string teamcityLauncherPath = UtilityFunctions.env("teamcity.dotnet.nunitlauncher");
				if(! string.IsNullOrEmpty(teamcityLauncherPath)) {
					path = teamcityLauncherPath;
					args.Add(teamCityArgs);
				} else {
					enableTeamCity = false;
				}
			}
			
			if (!string.IsNullOrEmpty(include)) {
				if(enableTeamCity) {
					args.Add(string.Format("/category-include:{0}", exclude));
				}
				else {
					args.Add(string.Format("/include:{0}", include));					
				}
			}
			else if (!string.IsNullOrEmpty(exclude)) {
				if(enableTeamCity) {
					args.Add(string.Format("/category-exclude:{0}", exclude));
				}
				else {
					args.Add(string.Format("/exclude:{0}", exclude));
				}
			}

			foreach (var assembly in assemblyPaths) {
				var nunitArgs = new List<string>(args);
				nunitArgs.Add(assembly);

				IOFunctions.exec(path, nunitArgs.JoinWith(" "));
			}
		}

		/// <summary>
		/// Executes nunit against an assembly
		/// </summary>
		/// <param name="assemblyPath">The assembly</param>
		/// <param name="options">Additional options: path (path to unit), [include (categories to run) | exclude (categories to not run)]
		public static void nunit(string assemblyPath, Hash options) {
			nunit(new[] {assemblyPath}, options);
		}
	}
}