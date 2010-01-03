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
	using System.Linq;
	using Core;
	using NUnit.Framework;

	[TestFixture]
	public class DslTester : ScriptTest {
		[Test]
		public void Loads_simple_script() {
			string path = "Scripts\\SingleTarget.boo";
			var script = Runner.GenerateBuildScript(path);
			script.Count().ShouldEqual(1);
		}

		[Test]
		public void Retrieves_target_name() {
			string path = "Scripts\\SingleTarget.boo";
			var script = Runner.GenerateBuildScript(path);
			script.GetTarget("default").Name.ShouldEqual("default");
		}

		[Test]
		public void Loads_multiple_targets() {
			string path = "Scripts\\Dependencies.boo";
			var script = Runner.GenerateBuildScript(path);
			script.Count().ShouldEqual(4);
		}

		[Test]
		public void Loads_dependencies() {
			string path = "Scripts\\Dependencies.boo";
			var script = Runner.GenerateBuildScript(path);
			var defaultTarget = script.GetTarget(ScriptModel.DefaultTargetName);
			var executionSequence = defaultTarget.GetExecutionSequence().ToList();
			executionSequence.Count.ShouldEqual(3);
			executionSequence[0].Name.ShouldEqual("compile");
			executionSequence[1].Name.ShouldEqual("test");
			executionSequence[2].Name.ShouldEqual("default");
		}

		[Test]
		public void Executes_target() {
			ScriptFile = "Scripts\\PrintsText.boo";
			Execute();
			AssertOutput("default:", "executing", "");
		}

		[Test]
		public void Executes_multiple_targets() {
			ScriptFile = "Scripts\\PrintsText.boo";
			Execute("default", "hello");
			AssertOutput("default:", "executing", "", "hello:", "hello", "");
		}

		[Test]
		public void Executes_target_from_within_target() {
			ScriptFile = "Scripts\\PrintsText.boo";
			Execute("helloWorld");
			AssertOutput("helloWorld:", "hello:", "hello", "", "world");
		}

		[Test]
		public void Calls_multiple_targets() {
			ScriptFile = "Scripts\\PrintsText.boo";
			Execute("helloWorldWithMultipleCalls");
			AssertOutput("helloWorldWithMultipleCalls:", "hello:", "hello", "", "world:", "world");
		}

		[Test]
		public void Loads_single_dependency() {
			ScriptFile = "Scripts\\Dependencies.boo";
			var script = Runner.GenerateBuildScript(ScriptFile);
			var target = script.GetTarget("oneDependency");
			var executionSequence = target.GetExecutionSequence().ToList();
			executionSequence.Count.ShouldEqual(2);
			executionSequence[0].Name.ShouldEqual("compile");
			executionSequence[1].Name.ShouldEqual("oneDependency");
		}
	}
}