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

namespace Phantom.Core.Language {
	using Boo.Lang.Compiler.Ast;
	using Boo.Lang.Compiler.Steps;

	public class ExpressionToDependencyNamesStep : AbstractTransformerCompilerStep {
		public override void Run() {
			Visit(CompileUnit);
		}

		public override void OnReferenceExpression(ReferenceExpression node) {
			if (node.ParentNode is ArrayLiteralExpression && PhantomDslEngine.IsTargetMethod(node.ParentNode.ParentNode)) {
				ReplaceCurrentNode(new StringLiteralExpression(node.Name));
			}
		}
	}
}