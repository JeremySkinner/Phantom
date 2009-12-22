#region License (+ashmind)

// Copyright Jeremy Skinner (http://www.jeremyskinner.co.uk)
// Copyright Andrey Shchekin (http://www.ashmind.com)
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
// The latest version of this file can be found at http://github.com/ashmind/Phantom

#endregion

namespace Phantom.Core.Integration {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Boo.Lang.Compiler.Ast;
    using Boo.Lang.Compiler.Steps;

    public class TaskImportStep : AbstractTransformerCompilerStep {
        private readonly ITaskImportBuilder[] importers;

        public TaskImportStep(params ITaskImportBuilder[] importers) {
            this.importers = importers.ToArray();
        }

        public override void Run() {
            this.Visit(this.CompileUnit);
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

        private IEnumerable<Import> ImportTasks(Import import) {
            if (import.AssemblyReference == null)
                throw new InvalidOperationException("Cannot import tasks: 'from' assembly is not specified.");
                       
            foreach (var importer in this.importers) {
                var newImport = importer.BuildImportFrom(import.AssemblyReference.Name);
                if (newImport == null)
                    continue;

                yield return newImport;
            }
        }
    }
}
