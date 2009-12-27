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
	using NUnit.Framework;

	[TestFixture]
	public class GlobalsTester : ScriptTest {
		const string expected = "0.1";

		[Test]
		public void Executes_global_function() {
			ScriptFile = "Scripts\\UsesGlobals.boo";
			Execute();
			AssertOutput("default:", expected);
		}

		[Test]
		public void Executes_global_function_from_imported_script() {
			ScriptFile = "Scripts\\UsesGlobals.boo";

			Execute("printVersion");
			AssertOutput("printVersion:", expected);
		}
	}
}