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

namespace Phantom.Core {
	using Boo.Lang.Compiler;
	using Boo.Lang.Compiler.Ast;
	using Boo.Lang.Compiler.Steps;
	using Builtins;
	using Rhino.DSL;

	public class PhantomDslEngine : DslEngine {
		protected override void CustomizeCompiler(BooCompiler compiler, CompilerPipeline pipeline, string[] urls) {
			pipeline.Insert(1, new ImplicitBaseClassCompilerStep(typeof (PhantomBase), "Execute", typeof (UtilityFunctions).Namespace));
			pipeline.Insert(2, new ExpressionToTargetNameStep());
			pipeline.Insert(3, new ExpressionToDependencyNamesStep());
			pipeline.Insert(4, new ExpressionToCallTargetNameStep());
			pipeline.Insert(5, new UseSymbolsStep());
			pipeline.Insert(6, new AutoReferenceFilesCompilerStep());
		}

		static bool IsTargetMethod(Node node) {
			var macro = node as MacroStatement;
			if (macro != null && macro.Name == "target") {
				return true;
			}
			return false;
		}

		static bool IsCallMethod(Node node) {
			var macro = node as MacroStatement;
			if (macro != null && macro.Name == "call") {
				return true;
			}
			return false;
		}

		class ExpressionToTargetNameStep : AbstractTransformerCompilerStep {
			public override void Run() {
				Visit(CompileUnit);
			}

			public override void OnReferenceExpression(ReferenceExpression node) {
				if (IsTargetMethod(node.ParentNode)) {
					ReplaceCurrentNode(new StringLiteralExpression(node.Name));
				}
			}
		}

		class ExpressionToDependencyNamesStep : AbstractTransformerCompilerStep {
			public override void Run() {
				Visit(CompileUnit);
			}

			public override void OnReferenceExpression(ReferenceExpression node) {
				if (node.ParentNode is ArrayLiteralExpression && IsTargetMethod(node.ParentNode.ParentNode)) {
					ReplaceCurrentNode(new StringLiteralExpression(node.Name));
				}
			}
		}

		class ExpressionToCallTargetNameStep : AbstractTransformerCompilerStep {
			public override void Run() {
				Visit(CompileUnit);
			}

			public override void OnReferenceExpression(ReferenceExpression node) {
				if (IsCallMethod(node.ParentNode)) {
					ReplaceCurrentNode(new StringLiteralExpression(node.Name));
				}
			}
		}
	}
}