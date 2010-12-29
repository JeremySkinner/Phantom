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
	using Integration;
	using Rhino.DSL;

	[Export]
	public class BuildRunner {
		readonly List<IDslFactory> _dsls = new List<IDslFactory>();

		[ImportingConstructor]
		public BuildRunner([ImportMany] IEnumerable<ITaskImportBuilder> taskImportBuilders) {
			_dsls.Add(new BooDslFactory(taskImportBuilders));
			_dsls.Add(new CSharpDslFactory());
		}

		public static BuildRunner Create() {
			var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var container = new CompositionContainer(new DirectoryCatalog(directory));
			return container.GetExportedValue<BuildRunner>();
		}

		public ScriptModel GenerateBuildScript(string path) {
			var factory = _dsls.First(x => x.CanGenerateDsl(path));
			return factory.GenerateScriptModel(path);
		}

		public void Execute(PhantomOptions options) {
			if (options.AttachDebugger && !Debugger.IsAttached)
				Debugger.Launch();

			var script = GenerateBuildScript(options.File);
			script.ExecuteTargets(options.TargetNames.ToArray());
		}

		public void OutputTargets(PhantomOptions options) {
			Console.WriteLine("Targets in {0}: ", options.File);
			var script = GenerateBuildScript(options.File);
			var allTargets = script.OrderBy(x => x.Name).ToList();

			int maxTargetLength = allTargets.Max(x => x.Name.Length);

			foreach (var target in script.OrderBy(x => x.Name)) {
				string name = target.Name.PadRight(maxTargetLength + 10, ' ');
				string description = target.Description ?? string.Empty;

				if (description.Length > 47) {
					description = description.Substring(0, 47) + "...";
				}

				Console.WriteLine(name + description);
			}
		}
	}
}