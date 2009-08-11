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
	public sealed class NUnitFunctions {

		/// <summary>
		/// Executes nunit against the specified assembly. Assumes nunit is located at lib\nunit\nunit-console.exe
		/// </summary>
		/// <param name="assembly"></param>
		public static void nunit(string assembly) {
			nunit(new[] { assembly });
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
		/// <param name="options">Additional options: path (path to unit)</param>
		public static void nunit(string[] assemblyPaths, Hash options) {
			string path = options.ObtainAndRemove("path", "lib\\nunit\\nunit-console.exe");
			foreach (var assembly in assemblyPaths) {
				 UtilityFunctions.exec(path, "\"" + assembly + "\"");
			}
		}
	}
}