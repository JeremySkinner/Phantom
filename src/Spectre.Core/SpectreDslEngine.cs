#region License

// Copyright 2009 Jeremy Skinner (http://www.jeremyskinner.co.uk)
//  
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
//  
// http://www.apache.org/licenses/LICENSE-2.0 
//  
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.
// 
// The latest version of this file can be found at http://github.com/JeremySkinner/Spectre

#endregion

namespace Spectre.Core {
	using System;
	using Boo.Lang.Compiler;
	using Boo.Lang.Compiler.Ast;
	using Boo.Lang.Compiler.Steps;
	using Rhino.DSL;

	public class SpectreDslEngine : DslEngine {
		protected override void CustomizeCompiler(BooCompiler compiler, CompilerPipeline pipeline, string[] urls) {
			pipeline.Insert(1, new ImplicitBaseClassCompilerStep(typeof (SpectreBase), "Execute"));
			pipeline.Insert(2, new ExpressionToTargetNameStep());
			pipeline.Insert(3, new ExpressionToDependencyNamesStep());
		}

		private static bool IsTargetMethod(Node node) {
			var macro = node as MacroStatement;
			if(macro != null &&   macro.Name == "target") {
				return true;
			}
			return false;
		}

		private class ExpressionToTargetNameStep : AbstractTransformerCompilerStep {
			public override void Run() {
				Visit(CompileUnit);
			}

			public override void OnReferenceExpression(ReferenceExpression node) {
				if(IsTargetMethod(node.ParentNode)) {
					ReplaceCurrentNode(new StringLiteralExpression(node.Name));
				}
			}
		}

		private class ExpressionToDependencyNamesStep : AbstractTransformerCompilerStep {
			public override void Run() {
				Visit(CompileUnit);
			}

			public override void OnReferenceExpression(ReferenceExpression node) {
				if(node.ParentNode is ArrayLiteralExpression && IsTargetMethod(node.ParentNode.ParentNode)) {
					ReplaceCurrentNode(new StringLiteralExpression(node.Name));		
				}
			}
		}
	}
}