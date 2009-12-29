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

	public class FileList : IEnumerable<WrappedFileSystemInfo> {
		readonly List<string> includes = new List<string>();
		readonly List<string> excludes = new List<string>();
		readonly string baseDir = "";
		private bool flatten;

		public FileList() {
			
		}

		public FileList(string baseDir) {
			this.baseDir = baseDir;
		}

		public FileList Flatten(bool flatten) {
			this.flatten = flatten;
			return this;
		}

		public FileList Exclude(string pattern) {
			excludes.Add(pattern);
			return this;
		}

		public FileList Include(string pattern) {
			includes.Add(pattern);
			return this;
		}

		private string FixupPath(string path) {
			//Glob likes forward slashes
			return Path.Combine(baseDir, path).Replace('\\', '/');
		}

		IEnumerable<WrappedFileSystemInfo> Execute() {
			var includedFiles = from include in includes
			                    from file in Glob.GlobResults(FixupPath(include))
			                    select file;

			var excludesFiles = from exclude in excludes
			                    from file in Glob.GlobResults(FixupPath(exclude))
			                    select file;

			foreach(var path in includedFiles.Except(excludesFiles)) {
				if(Directory.Exists(path)) {
					yield return new WrappedDirectoryInfo(baseDir, path, flatten);
				}
				else {
					yield return new WrappedFileInfo(baseDir, path, flatten);
				}
			}
		}

		public IEnumerator<WrappedFileSystemInfo> GetEnumerator() {
			return Execute().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}
}