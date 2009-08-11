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
	using System.Diagnostics;
	using System.Runtime.CompilerServices;

	[CompilerGlobalScope]
	public sealed class UtilityFunctions {
		/// <summary>
		/// Gets the current version of Spectre.
		/// </summary>
		public static string version() {
			return typeof (UtilityFunctions).Assembly.GetName().Version.ToString();
		}

		/// <summary>
		/// Gets an environment variable
		/// </summary>
		/// <param name="variableName">Name of the variable</param>
		/// <returns>The variable's value</returns>
		public static string env(string variableName) {
			return System.Environment.GetEnvironmentVariable(variableName);
		}

		/// <summary>
		/// Executes the specified program with the specified arguments
		/// </summary>
		/// <param name="command">The command to execute</param>
		/// <param name="args">Additional args</param>
		public static void exec(string command, string args) {
			var psi = new ProcessStartInfo(command, args) {
				UseShellExecute = false,
				RedirectStandardError = true
			};
			var process = Process.Start(psi);
			process.WaitForExit();
		}
	}
}