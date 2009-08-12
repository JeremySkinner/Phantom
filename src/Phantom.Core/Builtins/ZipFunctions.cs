namespace Spectre.Core.Builtins {
	using System;
	using System.Runtime.CompilerServices;
	using Ionic.Zip;

	[CompilerGlobalScope]
	public sealed class ZipFunctions {
		public static void zip(string directory, string zipFileName) {
			Console.WriteLine("Zipping '{0}' to '{1}'", directory, zipFileName);
			using(var zip = new ZipFile()) {
				zip.AddDirectory(directory);
				zip.Save(zipFileName);
			}
		}

	}
}