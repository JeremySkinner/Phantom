#region License

// Copyright Jeremy Skinner (http://www.jeremyskinner.co.uk)
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
// 
// The latest version of this file can be found at http://github.com/JeremySkinner/Phantom

#endregion

namespace Phantom.Tests {
	using System;
	using System.IO;
	using Core;
	using NUnit.Framework;

	[TestFixture]
	public class WithMacroTester {
		StringWriter writer;
		BuildRunner runner;

		[SetUp]
		public void Setup() {
			writer = new StringWriter();
			Console.SetOut(writer);
			runner = new BuildRunner();
		}


		[Test]
		public void Correctly_executes_statement_inside_With_macro_using_instance() {
			var options = new PhantomOptions() { File = "scripts\\WithMacroTest.boo"};
			options.AddTarget("withInstance");
			runner.Execute(options);
			writer.AssertOutput("withInstance:", "1");
		}

		[Test]
		public void Correctly_executes_statement_inside_With_macro_using_ctor() {
			var options = new PhantomOptions() { File = "scripts\\WithMacroTest.boo" };
			options.AddTarget("withCtor");
			runner.Execute(options);
			writer.AssertOutput("withCtor:", "1");
		}

		[Test]
		public void Correctly_executes_statement_inside_With_macro_using_variable_and_variable_is_maintained_afterwards() {
			var options = new PhantomOptions() { File = "scripts\\WithMacroTest.boo" };
			options.AddTarget("withAssignment");
			runner.Execute(options);
			writer.AssertOutput("withAssignment:", "1", "2");
		}
	}
}