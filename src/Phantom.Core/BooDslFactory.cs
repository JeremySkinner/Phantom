namespace Phantom.Core {
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using Integration;
	using Rhino.DSL;

	public class BooDslFactory : IDslFactory {
		readonly DslFactory dslFactory;

		public BooDslFactory(IEnumerable<ITaskImportBuilder> taskImportBuilders) {
			dslFactory = new DslFactory();
			dslFactory.Register<PhantomBase>(
				new PhantomDslEngine(taskImportBuilders.ToArray())
				);
		}

		public bool CanGenerateDsl(string path) {
			var ext = Path.GetExtension(path);
			return ext == ".boo";
		}

		public ScriptModel GenerateScriptModel(string path) {
			var script = dslFactory.Create<PhantomBase>(path);
			script.Execute();
			return script.Model;
		}
	}
}