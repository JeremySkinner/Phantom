namespace Phantom.Tests {
	using NUnit.Framework;

	[TestFixture]
	public class CSharpSimpleTest : ScriptTest {
		[Test]
		public void Runs_simple_script() {
			ScriptFile = "Scripts/CSharpSimple.cs";
			Execute("default");
			AssertOutput("default:", "Hello");
		}

		[Test]
		public void Gets_description() {
			ScriptFile = "Scripts/CSharpSimple.cs";
			Runner.OutputTargets(Options);
			AssertOutput(
				"Targets in Scripts/CSharpSimple.cs:",
				"default          Description"
				);
		}

		[Test]
		public void Dependencies() {
			ScriptFile = "Scripts/CSharpSimple.cs";
			Execute("depends");
			AssertOutput("default:", "Hello","", "depends:", "Depends");
		}
	}
}