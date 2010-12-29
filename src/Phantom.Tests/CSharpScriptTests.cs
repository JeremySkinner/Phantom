namespace Phantom.Tests {
	using System.IO;
	using Boo.Lang;
	using Core;
	using Core.Integration;
	using NUnit.Framework;

	[TestFixture]
	public class CSharpScriptTests {
		[Test]
		public void METHOD() {
			var runner = new BuildRunner(new List<ITaskImportBuilder>());
			runner.Execute(new PhantomOptions() { File = new FileInfo("scripts/Foo.cs").FullName});
		}
	}
}