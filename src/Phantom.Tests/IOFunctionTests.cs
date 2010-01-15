namespace Phantom.Tests {
    using System.IO;
    using Core.Builtins;
    using NUnit.Framework;
    using System;

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
            Assert.Throws(typeof (UnauthorizedAccessException), () => directory.Delete(true));
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