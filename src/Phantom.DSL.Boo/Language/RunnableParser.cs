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
	using System.Linq;
	using Boo.Lang.Compiler.Ast;
	using Boo.Lang.Compiler.TypeSystem;
	using Boo.Lang.Compiler.TypeSystem.Reflection;
	using Boo.Lang.Compiler.TypeSystem.Services;

	public class RunnableParser {
		readonly NameResolutionService nameResolutionService;

		public RunnableParser(NameResolutionService nameResolutionService) {
			this.nameResolutionService = nameResolutionService;
		}

		public bool IsRunnable(Expression expression) {
			var method = expression as MethodInvocationExpression;
			if (method == null) return false;

			var reference = method.Target as ReferenceExpression;
			if (reference == null) return false;

			var targetType = nameResolutionService.Resolve(reference.Name, EntityType.Type) as IType;
			if (targetType == null) return false;

			var interfaces = targetType.GetInterfaces();
			if (!interfaces.Any(IsRunnable)) return false;

			return true;
		}

		bool IsRunnable(IType @interface) {
			if (@interface.ConstructedInfo == null) // not a generic
				return false;

			var definitionAsExternal = @interface.ConstructedInfo.GenericDefinition as ExternalType;
			if (definitionAsExternal == null)
				return false;

			return definitionAsExternal.ActualType == typeof (IRunnable<>);
		}
	}
}