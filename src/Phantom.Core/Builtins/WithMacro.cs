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

namespace Phantom.Core.Builtins {
	using Boo.Lang.Compiler;
	using Boo.Lang.Compiler.Ast;

	/// <summary>
	/// The 'with' macro can be used to turn this:
	/// with Foo():
	///   .bar()
	///   .baz()
	/// 
	/// ...into this:
	/// 
	/// var foo = new Foo();
	/// foo.bar();
	/// foo.baz();
	/// 
	/// ...it also works with alternative syntaxes:
	/// 
	/// with f = Foo():
	///   .bar()
	///   .baz()
	/// 
	/// ... this means you can continue to access the 'f' variable after the with block has completed.
	/// 
	/// f = Foo()
	/// with f:
	///    .bar()
	///    .baz()
	/// 
	/// This would be much simpler if I wrote the macro in Boo rather than C#, but I thought this would be fun...
	/// </summary>
	public class WithMacro : AbstractAstMacro {

		public override Statement Expand(MacroStatement macro) {
			if(macro.Arguments.Count != 1) {
				throw new ScriptParsingException("'with' must be called with only a single argument followed by a block.");
			}

			var arg = macro.Arguments[0];
			var instanceExpression = ConvertExpressionToTemporaryVariable(arg, macro.Body);
			var visitor = new OmittedReferenceVisitor(instanceExpression);

			visitor.Visit(macro.Body);

			return macro.Body;
		}

        Expression ConvertExpressionToTemporaryVariable(Expression methodInvocation, Block block) {
			var temporaryVariable = new ReferenceExpression(Context.GetUniqueName("with"));

			var assignment = new BinaryExpression {
				Operator = BinaryOperatorType.Assign,
				Left = Expression.Lift(temporaryVariable),
				Right = methodInvocation
			};

			block.Insert(0, assignment);

			return temporaryVariable;
		}

		private class OmittedReferenceVisitor : DepthFirstTransformer {
			readonly Expression instanceExpr;

			public OmittedReferenceVisitor(Expression instanceExpr) {
				this.instanceExpr = instanceExpr;
			}

			public override void OnMemberReferenceExpression(MemberReferenceExpression node) {
				if (node.Target is OmittedExpression) {
					node.Target = instanceExpr;
				}
			}
		}
	}
}