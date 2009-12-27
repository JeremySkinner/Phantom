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

namespace Phantom.Core.Language {
	using System.Linq;
	using Boo.Lang.Compiler.Ast;
	using Boo.Lang.Compiler.Steps;
	using Boo.Lang.Compiler.TypeSystem;
	using Boo.Lang.Compiler.TypeSystem.Reflection;
	using Builtins;

	public class AutoRunAllRunnablesStep : AbstractTransformerCompilerStep {
		public override void Run() {
			this.Visit(this.CompileUnit);
		}

		public override void OnMethodInvocationExpression(MethodInvocationExpression node) {
			base.OnMethodInvocationExpression(node);

			var processed = this.AnalyzeAndProcess(node);
			if (processed != node)
				ReplaceCurrentNode(processed);
		}

		private MethodInvocationExpression AnalyzeAndProcess(MethodInvocationExpression node) {
			var reference = node.Target as ReferenceExpression;
			if (reference == null)
				return node;

			var targetType = NameResolutionService.Resolve(reference.Name, EntityType.Type) as IType;
			if (targetType == null)
				return node;

			var interfaces = targetType.GetInterfaces();
			if (!interfaces.Any(IsRunnable))
				return node;

			if (IsInsideWithBlock(node))
				return node;

			return new MethodInvocationExpression(
				new MemberReferenceExpression(node, "Run")
			);
		}

		private bool IsRunnable(IType @interface) {
			if (@interface.ConstructedInfo == null) // not a generic
				return false;

			var definitionAsExternal = @interface.ConstructedInfo.GenericDefinition as ExternalType;
			if (definitionAsExternal == null)
				return false;

			return definitionAsExternal.ActualType == typeof(IRunnable<>);
		}

		bool IsInsideWithBlock(MethodInvocationExpression expression) {
			return expression.ParentNode is WithMacro.WithBinaryExpression;
		}
	}


}