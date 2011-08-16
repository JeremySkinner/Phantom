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
	using System.IO;
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

		public static void unzip(string zipFileName, string outputDirectory) {
			Console.WriteLine("Unzipping '{0}' to '{1}'", zipFileName, outputDirectory);
			using (var zip = new ZipFile(zipFileName)) {
				zip.ExtractAll(outputDirectory);
			}
		}
	}
}