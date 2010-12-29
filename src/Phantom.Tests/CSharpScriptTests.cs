namespace Phantom.Tests {
	using System.IO;
	using Boo.Lang;
	using Core;
	using Core.Integration;
	using NUnit.Framework;

	[TestFixture]
	public class CSharpScriptTests : ScriptTest {
		[Test]
		public void Loads_csharp_script() {
			ScriptFile = "Scripts\\Foo.cs";
			Execute();
			AssertOutput("default:", "I am in yr default");
//			var runner = new BuildRunner(new List<ITaskImportBuilder>());
//			runner.Execute(new PhantomOptions() { File = new FileInfo("scripts/Foo.cs").FullName});
		}
	}
}