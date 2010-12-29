namespace Phantom.Core {
	using System.Collections.Generic;
	using System.ComponentModel.Composition;
	using System.IO;
	using System.Linq;
	using Integration;
	using Rhino.DSL;

	[Export(typeof(IDslFactory))]
	public class BooDslFactory : IDslFactory {
		readonly DslFactory dslFactory;

		[ImportingConstructor]
		public BooDslFactory([ImportMany] IEnumerable<ITaskImportBuilder> taskImportBuilders) {
			dslFactory = new DslFactory();
			dslFactory.Register<PhantomBase>(
				new PhantomDslEngine(taskImportBuilders.ToArray())
				);

		}

		public bool CanExecute(string path) {
			var ext = Path.GetExtension(path);
			return ext == ".boo";
		}

		public ScriptModel BuildModel(string path) {
			var script = dslFactory.Create<PhantomBase>(path);
			script.Execute();
			return script.Model;
		}
	}
}