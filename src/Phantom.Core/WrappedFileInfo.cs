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

namespace Phantom.Core
{
	using System.IO;

	public abstract class WrappedFileSystemInfo : FileSystemInfo
	{
		readonly FileSystemInfo inner;

		protected WrappedFileSystemInfo(string baseDir, string originalPath, FileSystemInfo inner, bool flatten)
		{
			this.inner = inner;
			BaseDir = baseDir;
			MatchedPath = originalPath;
			Flatten = flatten;
		}

		protected string BaseDir { get; private set; }
		protected string MatchedPath { get; private set; }
		protected bool Flatten { get; private set; }

		public override string Name
		{
			get { return inner.Name; }
		}

		public override bool Exists
		{
			get { return inner.Exists; }
		}

		public override string FullName
		{
			get { return inner.FullName; }
		}

		public string PathWithoutBaseDirectory
		{
			get { return MatchedPath.Substring(BaseDir.Length).Trim('/').Trim('\\'); }
		}

		public override void Delete()
		{
			inner.Delete();
		}

		public abstract void CopyToDirectory(string path);

		public override string ToString()
		{
			return FullName;
		}
	}

	public class WrappedFileInfo : WrappedFileSystemInfo
	{
		public WrappedFileInfo(string baseDir, string path, bool flatten)
			: base(baseDir, path, new FileInfo(path), flatten)
		{
		}

		public override void CopyToDirectory(string path)
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			if (Flatten)
			{
				var combinedPath = Path.Combine(path, Name);
				File.Copy(FullName, combinedPath, true);
			}
			else
			{
				var combinedPath = Path.Combine(path, PathWithoutBaseDirectory);
				var newPath = Path.GetDirectoryName(combinedPath);
				if (!Directory.Exists(newPath))
				{
					Directory.CreateDirectory(newPath);
				}
				File.Copy(FullName, combinedPath, true);
			}
		}
	}

	public class WrappedDirectoryInfo : WrappedFileSystemInfo
	{
		public WrappedDirectoryInfo(string baseDir, string path, bool flatten)
			: base(baseDir, path, new DirectoryInfo(path), flatten)
		{
		}

		public override void CopyToDirectory(string path)
		{
			if (Flatten) return; //copying directories is a no-op when flattened.
			var combinedPath = Path.Combine(path, PathWithoutBaseDirectory);
			if (!Directory.Exists(combinedPath))
			{
				Directory.CreateDirectory(combinedPath);
			}
		}
	}
}