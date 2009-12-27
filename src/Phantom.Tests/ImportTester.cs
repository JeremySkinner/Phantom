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
	public class ImportTester : ScriptTest {
		[Test]
		public void Imports_file() {
			ScriptFile = "Scripts\\Import.boo";
			Execute();
			AssertOutput("default:", "hello");
		}
	}
}