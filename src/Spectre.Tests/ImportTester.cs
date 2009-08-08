namespace Spectre.Tests {
	using System;
	using System.IO;
	using Core;
	using NUnit.Framework;

	[TestFixture]
	public class ImportTester {
		StringWriter writer;
		BuildRunner runner;

		[SetUp]
		public void Setup() {
			writer = new StringWriter();
			Console.SetOut(writer);

			runner = new BuildRunner();
		}

		[Test]
		public void Imports_file() {
			runner.Execute(new SpectreOptions() { File = "Scripts\\Import.boo"});
			writer.AssertOutput("hello");
		}
	}
}