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
	using System;
	using System.ComponentModel.Composition.Hosting;
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
				var container = new CompositionContainer(new DirectoryCatalog(Directory.GetCurrentDirectory()));
				Runner = container.GetExportedValue<BuildRunner>();
			}

			Options = new PhantomOptions();
			Writer = new StringWriter();
			Console.SetOut(Writer);

			Setup();
		}

		public virtual void Setup() {
		}

		protected string ScriptFile {
			get { return Options.File; }
			set { Options.File = value; }
		}

		public void Execute(params string[] targets) {
			foreach (var target in targets) {
				Options.AddTarget(target);
			}

			Runner.Execute(Options);
		}

		protected virtual void AssertOutput(params string[] lines) {
			Writer.AssertOutput(lines);
		}
	}
}