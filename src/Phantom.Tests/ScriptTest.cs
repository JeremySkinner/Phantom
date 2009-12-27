namespace Phantom.Tests {
	using System;
	using System.IO;
	using Core;
	using NUnit.Framework;

	[TestFixture]
	public abstract class ScriptTest {
		protected static BuildRunner Runner;
		protected PhantomOptions Options;
		protected StringWriter Writer;

		[SetUp]
		public void BaseSetup() {
			if (Runner == null) {
				Runner = BuildRunner.Create();
			}

			Options = new PhantomOptions();
			Writer = new StringWriter();
			Console.SetOut(Writer);

			Setup();
		}

		public virtual void Setup() { }

		protected string ScriptFile {
			get { return Options.File; }
			set { Options.File = value; }
		}

		public void Execute(params string[] targets) {
			foreach(var target in targets) {
				Options.AddTarget(target);
			}

			Runner.Execute(Options);
		}

		protected virtual void AssertOutput(params string[] lines) {
			Writer.AssertOutput(lines);
		}
	}
}