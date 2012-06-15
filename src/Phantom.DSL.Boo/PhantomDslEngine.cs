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

namespace Phantom.Core {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Boo.Lang.Compiler;
	using Boo.Lang.Compiler.Ast;
	using Boo.Lang.Compiler.Pipelines;
	using Boo.Lang.Compiler.Steps;
	using Builtins;
	using Integration;
	using Language;
	using Rhino.DSL;

	public class PhantomDslEngine : DslEngine, IIncludeCompiler {
		readonly IList<ITaskImportBuilder> importBuilders;
		bool InIncludeMode { get; set; }

		public PhantomDslEngine(IList<ITaskImportBuilder> importBuilders) {
			this.importBuilders = importBuilders;
		}

		protected override void CustomizeCompiler(BooCompiler compiler, CompilerPipeline pipeline, string[] urls) {
			var steps = new List<ICompilerStep>();
		
			steps.Add(new IncludeSupportStep(new PhantomDslEngine(importBuilders) {InIncludeMode = true}));
			if (!InIncludeMode) {
				steps.Add(new UnescapeNamesStep());
				steps.Add(new ExpressionToTargetNameStep());
				steps.Add(new ExpressionToDependencyNamesStep());
				steps.Add(new ExpressionToCleanupNameStep());
				steps.Add(new ExpressionToCallTargetNameStep());
				steps.Add(new AutoReferenceFilesCompilerStep());
				steps.Add(new TaskImportStep(importBuilders.ToArray()));

				steps.Add(new ImplicitBaseClassCompilerStep(typeof (PhantomBase), "Execute", typeof (UtilityFunctions).Namespace));
			}

			steps.Reverse();
			foreach (var step in steps) {
				pipeline.Insert(1, step);
			}

			if (!InIncludeMode)
				pipeline.InsertBefore(typeof (ProcessMethodBodiesWithDuckTyping), new AutoRunAllRunnablesStep());

			compiler.Parameters.References.Add(typeof(UtilityFunctions).Assembly);
		}

		// practically the same as ForceCompile, but without saving to disk
		CompilerContext IIncludeCompiler.CompileInclude(string url) {
			if (!InIncludeMode)
				throw new InvalidOperationException("Cannot CompileInclude when not in include mode.");

			var compiler = new BooCompiler {
			                               	Parameters = {OutputType = CompilerOutputType, GenerateInMemory = true, Pipeline = new Parse()}
			                               };
			CustomizeCompiler(compiler, compiler.Parameters.Pipeline, new[] {url});
			AddInput(compiler, url);
			var compilerContext = compiler.Run();
			if (compilerContext.Errors.Count != 0) {
				throw CreateCompilerException(compilerContext);
			}
			HandleWarnings(compilerContext.Warnings);
			return compilerContext;
		}

		// cannibalized from DslEngine
		void AddInput(BooCompiler compiler, string url) {
			var input = Storage.CreateInput(url);
			if (input == null)
				throw new InvalidOperationException("Got a null input for url: " + url);

			compiler.Parameters.Input.Add(input);
		}

		public static bool IsTargetMethod(Node node) {
			var macro = node as MacroStatement;
			if (macro != null && macro.Name == "target") {
				return true;
			}
			return false;
		}

		public static bool IsCleanupMethod(Node node)
		{
			var macro = node as MacroStatement;
			if (macro != null && macro.Name == "cleanup")
			{
				return true;
			}
			return false;
		}
	}
}