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
	public class GlobalsTester {
		BuildRunner runner;
		StringWriter writer;

		[SetUp]
		public void Setup() {
			writer = new StringWriter();
			runner = new BuildRunner();
			Console.SetOut(writer);
		}

		[Test]
		public void Executes_global_function() {
			string expected = typeof (BuildRunner).Assembly.GetName().Version.ToString();
			runner.Execute(new PhantomOptions {File = "Scripts\\UsesGlobals.boo"});
			writer.AssertOutput("default:", expected);
		}

		[Test]
		public void Executes_global_function_from_imported_script() {
			string expected = typeof (BuildRunner).Assembly.GetName().Version.ToString();

			var options = new PhantomOptions {File = "Scripts\\UsesGlobals.boo"};
			options.AddTarget("printVersion");

			runner.Execute(options);
			writer.AssertOutput("printVersion:", expected);
		}
	}
}