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

	public class ExpressionToCallTargetNameStep : AbstractTransformerCompilerStep {
		public override void Run() {
			Visit(CompileUnit);
		}

        public override void OnMemberReferenceExpression(MemberReferenceExpression node) {
            if (IsCallMethod(node.ParentNode)) {
                ReplaceCurrentNode(new StringLiteralExpression(node.ToCodeString()));
            }
        }

		public override void OnReferenceExpression(ReferenceExpression node) {
			if (IsCallMethod(node.ParentNode)) {
				ReplaceCurrentNode(new StringLiteralExpression(node.Name));
			}
		}

		static bool IsCallMethod(Node node) {
			var macro = node as MacroStatement;
			if (macro != null && macro.Name == "call") {
				return true;
			}
			return false;
		}
	}
}