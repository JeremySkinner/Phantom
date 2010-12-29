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

namespace Phantom.Core.Integration {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Boo.Lang.Compiler.Ast;
	using Boo.Lang.Compiler.Steps;

	public class TaskImportStep : AbstractTransformerCompilerStep {
		readonly ITaskImportBuilder[] importers;

		public TaskImportStep(params ITaskImportBuilder[] importers) {
			this.importers = importers.ToArray();
		}

		public override void Run() {
			Visit(CompileUnit);
		}

		public override void OnModule(Module node) {
			for (var i = 0; i < node.Imports.Count; i++) {
				var import = node.Imports[i];
				if (import.Namespace != "tasks")
					continue;

				var newImports = ImportTasks(import);
				node.Imports.RemoveAt(i);

				var newIndex = i;
				foreach (var newImport in newImports) {
					node.Imports.Insert(newIndex, newImport);
					newIndex += 1;
				}
			}

			base.OnModule(node);
		}

		IEnumerable<Import> ImportTasks(Import import) {
			if (import.AssemblyReference == null)
				throw new InvalidOperationException("Cannot import tasks: 'from' assembly is not specified.");

			return from importer in importers
			       from newImport in importer.BuildImportsFrom(import.AssemblyReference.Name)
			       select newImport;
		}
	}
}