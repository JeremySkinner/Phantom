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

namespace Phantom.Core {
	using System.IO;

	public class FileWrapper {
		readonly FileInfo file;

		public FileWrapper(string fullPath) : this(new FileInfo(fullPath)) {
		}

		public FileWrapper(FileInfo file) {
			this.file = file;
		}

		public string FullName {
			get { return file.FullName; }
		}

		public string Extension {
			get { return file.Extension; }
		}

		public void Delete() {
			file.Delete();
		}

		public string Name {
			get { return file.Name; }
		}

		public long Length {
			get { return file.Length; }
		}

		public string DirectoryName {
			get { return file.DirectoryName; }
		}

		public bool IsReadOnly {
			get { return file.IsReadOnly; }
			set { file.IsReadOnly = value; }
		}

		public override string ToString() {
			return file.FullName;
		}

		public void CopyTo(string folder) {
			string destination = Path.Combine(folder, file.Name);
			file.CopyTo(destination);
		}
	}
}