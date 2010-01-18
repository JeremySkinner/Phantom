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

	[TestFixture]
	public class WithMacroTester : ScriptTest {
		public override void Setup() {
			ScriptFile = "scripts\\WithMacroTest.boo";
		}

		[Test]
		public void Correctly_executes_statement_inside_With_macro_using_instance() {
			Execute("withInstance");
			AssertOutput("withInstance:", "1");
		}

		[Test]
		public void Correctly_executes_statement_inside_With_macro_using_ctor() {
			Execute("withCtor");
			AssertOutput("withCtor:", "1");
		}

		[Test]
		public void Correctly_executes_statement_inside_With_macro_using_variable_and_variable_is_maintained_afterwards() {
			Execute("withAssignment");
			AssertOutput("withAssignment:", "1", "2");
		}
	}
}