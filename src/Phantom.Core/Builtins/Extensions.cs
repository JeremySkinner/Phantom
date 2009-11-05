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
	using System.Collections.Generic;
	using System.IO;
	using System.Runtime.CompilerServices;

	[CompilerGlobalScope]
	public static class Extensions {
		public static void CopyToDir(this FileInfo file, string directory) {
			string destination = Path.Combine(directory, file.Name);
			file.CopyTo(destination);
		}

		public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
			foreach(var item in source) {
				action(item);
			}
		}
	}
}