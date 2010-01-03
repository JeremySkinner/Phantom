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

namespace Phantom.Tests {
	using System.IO;
	using NUnit.Framework;

	[TestFixture]
	public class FileCopyTester : ScriptTest {
		public override void Setup() {
			ScriptFile = "Scripts/FileCopy.boo";
		}

		[Test]
		public void Copies_file_to_output_directory() {
			Execute("copyFile");
			new FileInfo("copy_output/Test1.txt").Exists.ShouldBeTrue();
		}

		[Test]
		public void Copies_files_and_subdirectories_to_output_Directory() {
			Execute("copySubDirectories");

			new FileInfo("copy_output/SubDirectory2/Test1.txt").Exists.ShouldBeTrue();
			new FileInfo("copy_output/SubDirectory3/Test2.txt").Exists.ShouldBeTrue();
			new FileInfo("copy_output/SubDirectory3/Test3.txt").Exists.ShouldBeTrue();
			new FileInfo("copy_output/SubDirectory3/SubDirectory4/Test4.txt").Exists.ShouldBeTrue();
		}

		[Test]
		public void Copies_files_flattened() {
			Execute("copyFlattened");

			new FileInfo("copy_output/Test1.txt").Exists.ShouldBeTrue();
			new FileInfo("copy_output/Test2.txt").Exists.ShouldBeTrue();
			new FileInfo("copy_output/Test3.txt").Exists.ShouldBeTrue();
			new FileInfo("copy_output/Test4.txt").Exists.ShouldBeTrue();
		}
	}
}