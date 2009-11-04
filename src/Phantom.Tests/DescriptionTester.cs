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
	public class DescriptionTester {
		BuildRunner runner;
		TextWriter writer;

		[SetUp]
		public void Setup() {
			runner = new BuildRunner();
			writer = new StringWriter();
			Console.SetOut(writer);
		}

		[Test]
		public void Outputs_description() {
			runner.OutputTargets(new PhantomOptions {File = "Scripts\\Descriptions.boo"});
			writer.AssertOutput(
				"Targets in Scripts\\Descriptions.boo:",
				"compile          Compiles",
				"default          The default"
				);
		}

		[Test]
		public void Truncates_long_Descriptions() {
			runner.OutputTargets(new PhantomOptions {File = "Scripts\\LongDescription.boo"});
			writer.AssertOutput(
				"Targets in Scripts\\LongDescription.boo:",
				"default          The quick brown fox jumped over the lazy dog th..."
				);
		}
	}
}