using NUnit.Framework;
using Spectre.Core;

namespace Spectre.Tests {
	using System;
	using System.Linq;

	[TestFixture]
	public class SpectreBaseTester {
		SpectreBase script;

		[SetUp]
		public void Setup() {
			script = new TestScript();
		}

		[Test]
		public void Creates_target() {
			script.target("foo", () => {});
			script.Model.GetTarget("foo").ShouldNotBeNull();
		}

		[Test]
		public void Finds_default_target() {
			script.target("default", () => {});
			script.Model.DefaultTarget.ShouldNotBeNull();
		}

		[Test]
		public void No_default_target_returns_null() {
			script.Model.DefaultTarget.ShouldBeNull();
		}

		[Test]
		public void Does_not_allow_same_target_to_be_added_multiple_times() {
			script.target("foo", null);
			typeof (SpectreException).ShouldBeThrownBy(() => script.target("foo", null));
		}

		[Test]
		public void Gets_execution_sequence() {
			script.target("foo", null);
			var target = script.Model.GetTarget("foo");
			target.GetExecutionSequence().Count().ShouldEqual(1);
		}

		[Test]
		public void Adds_dependencies() {
			script.target("compile", null);
			script.target("test", null);
			script.target("deploy", new[] { "compile", "test" }, null);
			var target = script.Model.GetTarget("deploy");
			target.GetExecutionSequence().Count().ShouldEqual(3);
			target.GetExecutionSequence().ElementAt(0).Name.ShouldEqual("compile");
			target.GetExecutionSequence().ElementAt(1).Name.ShouldEqual("test");
			target.GetExecutionSequence().ElementAt(2).Name.ShouldEqual("deploy");

		}

		[Test]
		public void Gets_nested_dependencies() {
			script.target("compile", null);
			script.target("test", new[] { "compile" }, null);
			script.target("deploy", new[] {"test" }, null);

			var target = script.Model.GetTarget("deploy");
			target.GetExecutionSequence().Count().ShouldEqual(3);
			target.GetExecutionSequence().ElementAt(0).Name.ShouldEqual("compile");
			target.GetExecutionSequence().ElementAt(1).Name.ShouldEqual("test");
			target.GetExecutionSequence().ElementAt(2).Name.ShouldEqual("deploy");
		}


		[Test]
		public void Throws_exception_when_dependency_not_found() {
			script.target("test", new[] { "NOTFOUND" }, null);
			script.target("deploy", new[] { "test" }, null);
			var target = script.Model.GetTarget("deploy");
			typeof (SpectreException).ShouldBeThrownBy(() => target.GetExecutionSequence().ToList());
		}

		[Test]
		public void Handles_recursive_targets() {
			script.target("foo", new[] { "bar"}, null);
			script.target("bar", new[] { "foo"}, null);
			var target = script.Model.GetTarget("bar");
			typeof (SpectreException).ShouldBeThrownBy(() => target.GetExecutionSequence().ToList());
		}

		[Test]
		public void Handles_recursive_child_targets() {
			script.target("foo", new[] { "baz" }, null);
			script.target("baz", new[] { "foo" }, null);
			script.target("bar", new[] { "foo" }, null);
			var target = script.Model.GetTarget("bar");
			typeof(SpectreException).ShouldBeThrownBy(() => target.GetExecutionSequence().ToList());
		}

		[Test]
		public void Executes_target() {
			bool executed = false;
			script.target("foo", () => executed = true);
			script.Model.ExecuteTargets("foo");
			executed.ShouldBeTrue();
		}

		[Test]
		public void Throws_when_trying_to_execute_nonexistent_target() {
			typeof (SpectreException).ShouldBeThrownBy(() => script.Model.ExecuteTargets("foo"));
		}

		[Test]
		public void Executes_child_targets() {
			bool fooExecuted = false;
			bool barExecuted = false;
			bool defaultExecuted = false;

			script.target("foo", () => fooExecuted = true);
			script.target("bar", () => barExecuted = true);
			script.target("default", new[] { "foo", "bar" }, () => defaultExecuted = true);
			script.Model.ExecuteTargets(ScriptModel.DefaultTargetName);

			fooExecuted.ShouldBeTrue();
			barExecuted.ShouldBeTrue();
			defaultExecuted.ShouldBeTrue();

		}

		[Test]
		public void Executes_in_correct_order() {
			int fooExecuted = -1;
			int barExecuted = -1;
			int defaultExecuted = -1;
			int counter = 0;

			script.target("foo", () => fooExecuted = counter++);
			script.target("bar", () => barExecuted = counter++);
			script.target("default", new[] { "foo", "bar" }, () => defaultExecuted=counter++);
			script.Model.ExecuteTargets(ScriptModel.DefaultTargetName);

			fooExecuted.ShouldEqual(0);
			barExecuted.ShouldEqual(1);
			defaultExecuted.ShouldEqual(2);
		}


		private class TestScript : SpectreBase {
			public override void Execute() {
			}
		}
	}
}