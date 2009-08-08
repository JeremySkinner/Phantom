namespace Spectre.Tests {
	using System;
	using System.IO;
	using System.Linq;
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
			runner.OutputTargets(new SpectreOptions() { File = "Scripts\\Descriptions.boo"});
			AssertOutput(
				"Targets in Scripts\\Descriptions.boo:",
				"compile          Compiles",
				"default          The default"
			);
		}

		[Test]
		public void Truncates_long_Descriptions() {
			runner.OutputTargets(new SpectreOptions() { File = "Scripts\\LongDescription.boo"});
			AssertOutput(
				"Targets in Scripts\\LongDescription.boo:",
				"default          The quick brown fox jumped over the lazy dog th..."
			);
		}

		private void AssertOutput(params string[] lines) {
			var output = writer.ToString()
				.Split(new[] {"\r\n"}, StringSplitOptions.None)
				.ToArray(); 

			for(int i = 0; i < lines.Length; i++ ) {
				output[i].TrimEnd().ShouldEqual(lines[i]);
			}

		}

	}
}