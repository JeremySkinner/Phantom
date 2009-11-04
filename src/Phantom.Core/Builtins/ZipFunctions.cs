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
	using Ionic.Zip;

	[CompilerGlobalScope]
	public sealed class ZipFunctions {
		public static void zip(string directory, string zipFileName) {
			Console.WriteLine("Zipping '{0}' to '{1}'", directory, zipFileName);
			using (var zip = new ZipFile()) {
				zip.AddDirectory(directory);
				zip.Save(zipFileName);
			}
		}
	}
}