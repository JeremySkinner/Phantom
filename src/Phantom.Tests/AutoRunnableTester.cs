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
	using NUnit.Framework;

	public class AutoRunTester : ScriptTest {
		public override void Setup() {
			ScriptFile = "scripts/AutoRunnable.boo";
		}

		[Test]
		public void Automatically_invokes_run_on_classes_that_implement_IRunnable() {
			Execute("autoRuns");
			AssertOutput("autoRuns:", "Executed");
		}

		[Test]
		public void Does_not_automatically_invoke_run_on_classes_that_implement_IRunnable_inside_a_With_block_until_block_complete() {
			Execute("autoRunWith");
			AssertOutput("autoRunWith:", "foo Executed");
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