namespace Phantom.Core.Builtins
{
	using System.Collections.Generic;
	using System.IO;
	using Language;
	using System.Linq;
	using System;

	public abstract class TestRunner<T> : IRunnable<T> where T : TestRunner<T>
	{
		public string toolPath { get; set; }
		public string[] assemblies { get; set; }
		public string assembly { get; set; }

		protected abstract void Execute();

		protected void Execute(string args) {
			if (!File.Exists(toolPath)) {
				throw new FileNotFoundException(string.Format("Could not execute the file '{0}' as it does not exist.", toolPath));
			}

			IOFunctions.exec(toolPath, args);
		}

		public T Run() {
			if (toolPath == null) {
				throw new InvalidOperationException("toolPath must be specified.");
			}

			if ((assemblies == null || assemblies.Length == 0) && string.IsNullOrEmpty(assembly)) {
				throw new InvalidOperationException("Please specify either the 'assembly' or the 'assemblies' to run your tests.");
			}

			if (!string.IsNullOrEmpty(assembly)) {
				assemblies = new[] {assembly};
			}

			// Enables wildcard in assembly selection.
			BuildAssemblyComposition(assemblies);

			Execute();
			return (T) this;
		}

		private void BuildAssemblyComposition(IEnumerable<string> inputAssemblies) {
			var assemblyBag = new FileList();
			foreach (var asm in inputAssemblies) {
				assemblyBag.Include(asm);
			}
			assemblies = assemblyBag
				.Select(x => x.FullName)
				.Distinct()
				.ToArray();
		}
	}
}
