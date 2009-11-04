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
	using System.Linq;

	public class FileList {
		List<string> includes = new List<string>();

		public static FileList Create(Action<FileList> configurator) {
			var list = new FileList();
			configurator(list);
			return list;
		}

		public void Include(string pattern) {
			includes.Add(pattern);
		}

		public void ForEach(Action<FileWrapper> forEachFile) {
			var files = from include in includes
			            from file in Glob.GlobResults(include)
			            select file;

			foreach(var file in files) {
				forEachFile(new FileWrapper(file));
			}
		}
	}
}