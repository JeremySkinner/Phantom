namespace Spectre.Core {
	using System;
	using Rhino.DSL;

	public class BuildRunner {
		static readonly DslFactory dslFactory;

		static BuildRunner() {
			dslFactory = new DslFactory();
			dslFactory.Register<SpectreBase>(new SpectreDslEngine());
		}

		public ScriptModel GenerateBuildScript(string path) {
			var script = dslFactory.Create<SpectreBase>(path);
			script.Execute();
			return script.Model;
		}
	}
}