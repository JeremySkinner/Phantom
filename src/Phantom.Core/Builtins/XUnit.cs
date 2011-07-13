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

namespace Phantom.Core.Builtins {
	using System.Collections.Generic;

	public class xunit : TestRunner<xunit> {
		public xunit() {
			toolPath = "lib/xunit/xunit.console.exe";
		}

		protected override void Execute() {
			var args = new List<string>();

			foreach (var asm in assemblies) {
				var xunitArgs = new List<string>(args) {asm};
				Execute(xunitArgs.JoinWith(" "));
			}
		}
	}
}