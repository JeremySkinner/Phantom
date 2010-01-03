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
	public class IncludeTester : ScriptTest {
		public override void Setup() {
			ScriptFile = "Scripts\\Include.boo";
		}

		[Test]
		public void Includes_file_globals() {
			Execute("globals");
			AssertOutput("globals:", "test");
		}

		[Test]
		public void Includes_file_methods() {
			Execute("methods");
			AssertOutput("methods:", "hello");
		}
	}
}