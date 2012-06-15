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

namespace Phantom.Core {
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.Composition;
	using System.ComponentModel.Composition.Hosting;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Reflection;

	[Export]
	public class BuildRunner {
		readonly IEnumerable<IDslFactory> dslFactories;
		readonly Func<TextWriter> log;

		protected TextWriter Log {
			get { return log();  }
		}

		public BuildRunner(IEnumerable<IDslFactory> dslFactories, Func<TextWriter> log) {
			this.dslFactories = dslFactories;
			this.log = log;
		}


		[ImportingConstructor]
		public BuildRunner([ImportMany] IEnumerable<IDslFactory> dslFactories) : this(dslFactories, ()=>Console.Out) {
		}

		public static BuildRunner Create() {
			var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var container = new CompositionContainer(new DirectoryCatalog(directory));
			return container.GetExportedValue<BuildRunner>();
		}

		public ScriptModel GenerateBuildScript(string path) {
			var factory = dslFactories.FirstOrDefault(x => x.CanExecute(path));
			if (factory == null) throw new ScriptLoadException(path);

			var model = factory.BuildModel(path);
			return model;
		}

		public void Execute(PhantomOptions options) {
			if (options.AttachDebugger && !Debugger.IsAttached)
				Debugger.Launch();

			// Copy additional args to environment variables
			//TODO: Don't use Environment to do this...
			foreach(var pair in options.AdditionalArguments) {
				Environment.SetEnvironmentVariable(pair.Key, pair.Value);
			}

			var script = GenerateBuildScript(options.File);
			script.Log = Log;
			script.ExecuteTargets(options.TargetNames.ToArray());
			script.ExecuteCleanups();
		}

		public void OutputTargets(PhantomOptions options) {
			Log.WriteLine("Targets in {0}: ", options.File);
			var script = GenerateBuildScript(options.File);
			var allTargets = script.OrderBy(x => x.Name).ToList();

			int maxTargetLength = allTargets.Max(x => x.Name.Length);

			foreach (var target in script.OrderBy(x => x.Name)) {
				string name = target.Name.PadRight(maxTargetLength + 10, ' ');
				string description = target.Description ?? string.Empty;

				if (description.Length > 47) {
					description = description.Substring(0, 47) + "...";
				}

				Log.WriteLine(name + description);
			}
		}
	}
}