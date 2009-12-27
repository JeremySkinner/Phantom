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

	public class DescriptionTester : ScriptTest {
		[Test]
		public void Outputs_description() {
			ScriptFile = "Scripts/Descriptions.boo";
			Runner.OutputTargets(Options);

			AssertOutput(
				"Targets in Scripts/Descriptions.boo:",
				"compile          Compiles",
				"default          The default"
				);
		}

		[Test]
		public void Truncates_long_Descriptions() {
			ScriptFile = "Scripts/LongDescription.boo";
			Runner.OutputTargets(Options);
			AssertOutput(
				"Targets in Scripts/LongDescription.boo:",
				"default          The quick brown fox jumped over the lazy dog th..."
				);
		}
	}
}