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
	using Boo.Lang.Compiler.Ast;
	using Boo.Lang.Compiler.Steps;
	using Builtins;

	public class AutoRunAllRunnablesStep : AbstractTransformerCompilerStep {
		public override void Run() {
			Visit(CompileUnit);
		}

		public override void OnMethodInvocationExpression(MethodInvocationExpression node) {
			base.OnMethodInvocationExpression(node);

			var processed = AnalyzeAndProcess(node);

			if (processed != node) {
				ReplaceCurrentNode(processed);
			}
		}

		MethodInvocationExpression AnalyzeAndProcess(MethodInvocationExpression node) {
			var runnableParser = new RunnableParser(NameResolutionService);

			if (! runnableParser.IsRunnable(node)) {
				return node;
			}

			if (IsInsideWithBlock(node)) {
				return node;
			}

			return new MethodInvocationExpression(
				new MemberReferenceExpression(node, "Run")
				);
		}

		bool IsInsideWithBlock(MethodInvocationExpression expression) {
			return expression.ParentNode is WithMacro.WithBinaryExpression;
		}
	}
}