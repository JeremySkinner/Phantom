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

	public class Cleanup {
		readonly Action block;
		readonly ScriptModel parentScript;


		public Cleanup(string name, Action block, string description, ScriptModel parentScript) {
			Name = name;
			Description = description;
			this.block = block;
			this.parentScript = parentScript;
		}

		public string Name { get; private set; }
		public string Description { get; private set; }

		public void Execute() {
			if (block != null) {
				block();
			}
		}
	}
}