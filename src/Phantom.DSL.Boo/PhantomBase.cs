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

	public abstract class PhantomBase {
		string currentDescription = null;

		readonly ScriptModel model = new ScriptModel();

		public void target(string name, Action block) {
			model.AddTarget(name, null, block, currentDescription);
			currentDescription = null;
		}

		public void target(string name, string[] dependencies, Action block) {
			model.AddTarget(name, dependencies, block, currentDescription);
			currentDescription = null;
		}

		public void target(string name, string dependency, Action block) {
			model.AddTarget(name, new[] {dependency}, block, currentDescription);
			currentDescription = null;
		}

		public void cleanup(string name, Action block){
			model.AddCleanup(name, block, currentDescription);
			currentDescription = null;
		}

		public void cleanup( Action block)
		{
			model.AddCleanup(null, block, currentDescription);
			currentDescription = null;
		}

		public abstract void Execute();

		public ScriptModel Model {
			get { return model; }
		}

		public void desc(string description) {
			currentDescription = description;
		}

		public void call(params string[] name) {
			model.ExecuteTargets(name);
		}
	}
}