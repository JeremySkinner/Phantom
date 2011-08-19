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
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Runtime.CompilerServices;

	public class FileFilter {
		readonly List<string> includes = new List<string>();
		readonly List<string> excludes = new List<string>();

		public FileFilter Exclude(string pattern) {
			excludes.Add(pattern);
			return this;
		}

		public FileFilter Include(string pattern) {
			includes.Add(pattern);
			return this;
		}

		public FileFilter IncludeEverything() {
			includes.Add("**/*");
			return this;
		}

		public FileFilter IncludeEveryThingInDirectory(string path) {
			if (!path.EndsWith("/") || !path.EndsWith("\\"))
				path += "/";
			includes.Add(path + "**/*");
			return this;
		}

		void CopyToDirectory(string sourceDirectory, string destinationDirectory, bool overwrite) {
			foreach (WrappedFileSystemInfo fileSystemInfo in GetFilesAndFolders(sourceDirectory)) {
				if (fileSystemInfo is WrappedDirectoryInfo) {

					var combinedPath = Path.Combine(destinationDirectory, fileSystemInfo.PathWithoutBaseDirectory);
					if (!Directory.Exists(combinedPath)) {
						Directory.CreateDirectory(combinedPath);
					}
				}
				else {
					if (!Directory.Exists(destinationDirectory)) {
						Directory.CreateDirectory(destinationDirectory);
					}

					var combinedPath = Path.Combine(destinationDirectory, fileSystemInfo.PathWithoutBaseDirectory);
					var newPath = Path.GetDirectoryName(combinedPath);
					if (!Directory.Exists(newPath)) {
						Directory.CreateDirectory(newPath);
					}
					File.Copy(fileSystemInfo.FullName, combinedPath, overwrite);
				}

			}
		}

		public FileFilter CopyToDirectory(string sourceDirectory, string destinationDirectory) {
			CopyToDirectory(sourceDirectory, destinationDirectory, true);
			return this;
		}

		public FileFilter CopyToDirectoryWithoutOverWrite(string sourceDirectory, string destinationDirectory) {
			CopyToDirectory(sourceDirectory, destinationDirectory, false);
			return this;
		}

		public static string PathWithoutBaseDirectory(string path, string baseDir) {
			if (path.StartsWith(baseDir))
				return path.Substring(baseDir.Length).Trim('/').Trim('\\');
			return path;
		}

		public void DeleteInDirectory(string sourceDirectory) {
			foreach (WrappedFileSystemInfo fileSystemInfo in GetFilesAndFolders(sourceDirectory)) {
				var fullPath = Path.Combine(sourceDirectory, fileSystemInfo.PathWithoutBaseDirectory);

				if (Directory.Exists(fullPath)) {
					Directory.Delete(fullPath, true);
					continue;
				}

				if (File.Exists(fullPath)) {
					File.Delete(fullPath);
					continue;
				}
			}
		}

		static FileAttributes RemoveAttribute(FileAttributes attributes, FileAttributes attributesToRemove) {
			return attributes & ~attributesToRemove;
		}

		public void ForcedDeleteInDirectory(string sourceDirectory) {
			foreach (WrappedFileSystemInfo fileSystemInfo in GetFilesAndFolders(sourceDirectory)) {
				var fullPath = Path.Combine(sourceDirectory, fileSystemInfo.PathWithoutBaseDirectory);

				if (File.Exists(fullPath)) {
					FileAttributes attributes = File.GetAttributes(fullPath);
					attributes = RemoveAttribute(attributes, FileAttributes.ReadOnly);
					File.SetAttributes(fullPath, attributes | FileAttributes.Normal);
					File.Delete(fullPath);
					continue;
				}
			}

			foreach (WrappedFileSystemInfo fileSystemInfo in GetFilesAndFolders(sourceDirectory)) {
				var fullPath = Path.Combine(sourceDirectory, fileSystemInfo.PathWithoutBaseDirectory);

				if (Directory.Exists(fullPath)) {
					Directory.Delete(fullPath, true);
					continue;
				}
			}

		}

		static string FixupPath(string baseDir, string path) {
			//Glob likes forward slashes
			return Path.Combine(baseDir, path).Replace('\\', '/');
		}

		public IEnumerable<WrappedFileSystemInfo> GetFilesAndFolders(string baseDir) {
			var includedFiles = from include in includes
			                    from file in Glob.GlobResults(FixupPath(baseDir, include))
			                    select file;

			var excludesFiles = from exclude in excludes
			                    from file in Glob.GlobResults(FixupPath(baseDir, exclude))
			                    select file;

			foreach (var path in includedFiles.Except(excludesFiles)) {
				if (Directory.Exists(path)) {
					yield return new WrappedDirectoryInfo(baseDir, path, false);
				}
				else {
					yield return new WrappedFileInfo(baseDir, path, false);
				}
			}
		}

		[CompilerGlobalScope]
		public static class FilesContainer {
			public static FileFilter NewFileFilter {
				get { return new FileFilter(); }
			}
		}

	}
}