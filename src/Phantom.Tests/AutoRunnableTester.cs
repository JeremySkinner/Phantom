namespace Phantom.Tests {
	using System;
	using System.IO;
	using Core;
	using NUnit.Framework;

	[TestFixture]
	public class AutoRunTester {
		BuildRunner runner;
		StringWriter writer;
		PhantomOptions options;

		[SetUp]
		public void Setup() {
			runner = new BuildRunner();
			writer = new StringWriter();
			options = new PhantomOptions() { File = "scripts/AutoRunnable.boo" };
			Console.SetOut(writer);
		}

		[Test]
		public void Automatically_invokes_run_on_classes_that_implement_IRunnable() {
			options.AddTarget("autoRuns");
			runner.Execute(options);
			writer.AssertOutput("autoRuns:", "Executed");
		}

		[Test]
		public void Does_not_automatically_invoke_run_on_classes_that_implement_IRunnable_inside_a_With_block_until_block_complete() {
			options.AddTarget("autoRunWith");
			runner.Execute(options);
			writer.AssertOutput("autoRunWith:", "foo Executed");
		}
	}
}

namespace Phantom.Tests.ForAutoRunTests {
	using System;
	using Core.Language;

	public class FooRunnable : IRunnable<FooRunnable> {
		string message;

		public void SetMessage(string message) {
			this.message = message;
		}

		public FooRunnable Run() {
			Console.WriteLine(message + "Executed");
			return this;
		}
	}
}