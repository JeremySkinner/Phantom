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
    using System;
    using System.Collections.Generic;
    using Boo.Lang.Compiler;
	using Boo.Lang.Compiler.Ast;
	using Boo.Lang.Compiler.Pipelines;
	using Boo.Lang.Compiler.Steps;

	using Builtins;
    using Builtins.IncludeSupport;
       
    using Rhino.DSL;

	public class PhantomDslEngine : DslEngine, IIncludeCompiler {
        private bool IncludeMode { get; set; }

	    protected override void CustomizeCompiler(BooCompiler compiler, CompilerPipeline pipeline, string[] urls) {
	        var index = 1;
            if (!this.IncludeMode) {
                pipeline.Insert(index, new ImplicitBaseClassCompilerStep(typeof (PhantomBase), "Execute", typeof (UtilityFunctions).Namespace));
                index += 1;
            }

            pipeline.Insert(index, new ExpressionToTargetNameStep());
            pipeline.Insert(index+1, new ExpressionToDependencyNamesStep());
            pipeline.Insert(index+2, new ExpressionToCallTargetNameStep());
            pipeline.Insert(index+3, new UseSymbolsStep());
            pipeline.Insert(index+4, new AutoReferenceFilesCompilerStep());
            pipeline.Insert(index+5, new AddIncludeSupportStep(new PhantomDslEngine { IncludeMode = true }));
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

        // practically the same as ForceCompile, but without saving to disk
        CompilerContext IIncludeCompiler.CompileInclude(string url) {
            if (!this.IncludeMode)
                throw new InvalidOperationException("Cannot CompileInclude when not in include mode.");

            var compiler = new BooCompiler {
                Parameters = { OutputType = this.CompilerOutputType, GenerateInMemory = true, Pipeline = new Parse() }
            };
            this.CustomizeCompiler(compiler, compiler.Parameters.Pipeline, new[] { url });
            this.AddInput(compiler, url);
            var compilerContext = compiler.Run();
            if (compilerContext.Errors.Count != 0) {
                throw this.CreateCompilerException(compilerContext);
            }
            this.HandleWarnings(compilerContext.Warnings);
            return compilerContext;
        }

        // cannibalized from DslEngine
        private void AddInput(BooCompiler compiler, string url) {
            var input = this.Storage.CreateInput(url);
            if (input == null)
                throw new InvalidOperationException("Got a null input for url: " + url);

            compiler.Parameters.Input.Add(input);
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