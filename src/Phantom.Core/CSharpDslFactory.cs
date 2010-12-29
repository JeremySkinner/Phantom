namespace Phantom.Core {
	using System;
	using System.CodeDom.Compiler;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using Microsoft.CSharp;

	public class CSharpDslFactory : IDslFactory {
		public bool CanGenerateDsl(string path) {
			var ext = Path.GetExtension(path);
			return ext == ".cs";
		}

		public ScriptModel GenerateScriptModel(string path) {
			var csc = new CSharpCodeProvider(new Dictionary<string, string> { });
			string pathToPhantomCore = GetType().Assembly.Location;

			var parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll", pathToPhantomCore }, "BuildScript1.dll", true);
			parameters.GenerateInMemory = true;

			var results = csc.CompileAssemblyFromFile(parameters, path);
			var compilerErrors = results.Errors.OfType<CompilerError>().ToList();

			if(compilerErrors.Any()) {
				foreach(var error in compilerErrors) {
					Console.WriteLine(error.ErrorText);
				}

				throw new InvalidOperationException("Could not compile csharp script.");
			}

			var types = from type in results.CompiledAssembly.GetTypes()
			            where typeof(BuildScript).IsAssignableFrom(type)
			            where !type.IsAbstract
			            let script = (BuildScript)Activator.CreateInstance(type)
			            let model = script.GetScriptModel()
			            select model;

			return types.Single();
		}
	}
}