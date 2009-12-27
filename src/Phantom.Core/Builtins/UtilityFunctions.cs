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

	[CompilerGlobalScope]
	public sealed class UtilityFunctions {
		/// Gets an environment variable
		/// </summary>
		/// <param name="variableName">Name of the variable</param>
		/// <returns>The variable's value</returns>
		public static string env(string variableName) {
			return Environment.GetEnvironmentVariable(variableName);
		}
	}
}