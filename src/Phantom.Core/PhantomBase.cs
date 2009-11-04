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

	public abstract class PhantomBase {
		readonly ScriptModel model = new ScriptModel();

		public void target(string name, Action block) {
			model.AddTarget(name, null, block);
		}

		public void target(string name, string[] dependencies, Action block) {
			model.AddTarget(name, dependencies, block);
		}

		public void target(string name, string dependency, Action block) {
			model.AddTarget(name, new[] {dependency}, block);
		}

		public abstract void Execute();

		public ScriptModel Model {
			get { return model; }
		}

		public void desc(string description) {
			model.SetCurrentDescription(description);
		}

		public void call(params string[] name) {
			model.ExecuteTargets(name);
		}
	}
}