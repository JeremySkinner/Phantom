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
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	public class FileList : IEnumerable<FileInfo> {
		readonly List<string> includes = new List<string>();

		public FileList Include(string pattern) {
			includes.Add(pattern);
			return this;
		}

		IEnumerable<FileInfo> Execute() {
			return from include in includes
			       from file in Glob.GlobResults(include)
			       select new FileInfo(file);
		}

		public IEnumerator<FileInfo> GetEnumerator() {
			return Execute().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}
}