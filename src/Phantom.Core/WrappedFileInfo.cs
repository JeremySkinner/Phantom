namespace Phantom.Core {
	using System;
	using System.IO;

	public abstract class WrappedFileSystemInfo : FileSystemInfo {
		readonly FileSystemInfo inner;
		protected string BaseDir { get; private set; }
		protected string MatchedPath { get; private set; }
		protected bool Flatten { get; private set; }

		protected WrappedFileSystemInfo(string baseDir, string originalPath, FileSystemInfo inner, bool flatten) {
			this.inner = inner;
			BaseDir = baseDir;
			this.MatchedPath = originalPath;
			Flatten = flatten;
		}

		public override void Delete() {
			inner.Delete();
		}

		public override string Name {
			get { return inner.Name; }
		}

		public override bool Exists {
			get { return inner.Exists; }
		}

		public override string FullName {
			get { return inner.FullName; }
		}

		public abstract void CopyToDirectory(string path);

		public override string ToString() {
			return FullName;
		}

		protected string PathWithoutBaseDirectory {
			get {
				return MatchedPath.Substring(BaseDir.Length).Trim('/');
			}
		}
	}

	public class WrappedFileInfo : WrappedFileSystemInfo {
		public WrappedFileInfo(string baseDir, string path, bool flatten) : base(baseDir, path, new FileInfo(path), flatten) {
			
		}

		public override void CopyToDirectory(string path) {
			if (!Directory.Exists(path)) {
				Directory.CreateDirectory(path);
			}

			if (Flatten) {
				var combinedPath = Path.Combine(path, Name);
				File.Copy(FullName, combinedPath, true);
			}
			else {
				var combinedPath = Path.Combine(path, PathWithoutBaseDirectory);
				File.Copy(FullName, combinedPath, true);
			}
			//ensure all segments of "path" exist
			//all subdirectories in current file's path
 
			//baseDir -> "SubDirectory"
			//path -> SubDirectory/SubDirectory2/Foo.txt
			//FullPath....
			//pathWithoutBaseDir -> SubDirectory2/Foo.txt
		}
	}

	public class WrappedDirectoryInfo : WrappedFileSystemInfo {
		public WrappedDirectoryInfo(string baseDir, string path, bool flatten) : base(baseDir, path, new DirectoryInfo(path), flatten) {
			
		}

		public override void CopyToDirectory(string path) {
			if(Flatten) return; //copying directories is a no-op when flattened.

			var combinedPath = Path.Combine(path, PathWithoutBaseDirectory);
			if(! Directory.Exists(combinedPath)) {
				Directory.CreateDirectory(combinedPath);
			}
		}
	}
}