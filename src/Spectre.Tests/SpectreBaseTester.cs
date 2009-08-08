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

		private class TestScript : SpectreBase {
			public override void Execute() {
				
			}
		}
	}
}