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

namespace Phantom.Core.Language {
	using System.IO;
	using System.Linq;
	using Boo.Lang.Compiler.Ast;
	using Boo.Lang.Compiler.Steps;

	public class IncludeSupportStep : AbstractTransformerCompilerStep {
		readonly IIncludeCompiler compiler;

		public IncludeSupportStep(IIncludeCompiler compiler) {
			this.compiler = compiler;
		}

		public override void Run() {
			Visit(CompileUnit);
		}

		public override void OnMacroStatement(MacroStatement node) {
			base.OnMacroStatement(node);

			if (node.Name == "include")
				ExpandIncludeMacro(node);
		}

		void ExpandIncludeMacro(MacroStatement macro) {
			if (macro.Arguments.Count != 1)
				throw new ScriptParsingException("include requires a single literal string argument ('filename').");

			var include = macro.Arguments[0] as StringLiteralExpression;
			if (include == null)
				throw new ScriptParsingException("include argument should be a literal string ('filename').");

			var fullPath = Path.Combine(
				Path.GetDirectoryName(macro.LexicalInfo.FullPath),
				include.Value
				);

			var compiled = compiler.CompileInclude(fullPath).CompileUnit.Modules[0];

			var module = macro.GetAncestor<Module>();
			foreach (var import in compiled.Imports) {
				module.Imports.Add(import);
			}

			var type = macro.GetAncestor<TypeDefinition>();
			foreach (var member in compiled.Members) {
				type.Members.Add(member);
			}

			var parent = (Block) macro.ParentNode;
			var currentPosition = parent.Statements.IndexOf(macro);

			RemoveCurrentNode();
			foreach (var global in compiled.Globals.Statements.Reverse()) {
				parent.Insert(currentPosition, global);
			}
		}
	}
}