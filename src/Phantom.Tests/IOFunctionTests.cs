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
	using System;
	using System.IO;
	using Core.Builtins;
	using NUnit.Framework;

	[TestFixture]
	public class IOFunctionTests : ScriptTest {
		public override void Setup() {
			ScriptFile = "Scripts/FileCopy.boo";
		}

		[Test]
		public void Directories_with_readonly_files_throws_UnauthorizedAccessException_while_deleted() {
			Execute("copyViews");
			var fileInfo = new FileInfo("copy_Views/Shared/Error.aspx");
			fileInfo.Exists.ShouldBeTrue();
			fileInfo.Attributes = FileAttributes.ReadOnly;
			var directory = fileInfo.Directory;

			typeof(UnauthorizedAccessException).ShouldBeThrownBy(() => directory.Delete(true));
		}

		[Test]
		public void rm_should_delete_all_subdirectories_without_throwing() {
			Execute("copyViews");
			var fileInfo = new FileInfo("copy_Views/Shared/Error.aspx");
			fileInfo.Exists.ShouldBeTrue();
			fileInfo.Attributes = FileAttributes.ReadOnly;
			IOFunctions.rm("copy_Views");
			new DirectoryInfo(@"copy_Views/Shared").Exists.ShouldEqual(false);
		}
	}
}