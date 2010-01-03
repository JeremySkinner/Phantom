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
	using System;
	using System.Collections.Generic;

	public class xunit : ExecutableTool<xunit> {
		public xunit() {
			toolPath = "lib/xunit/xunit.console.exe";
		}

		public string[] assemblies { get; set; }
		public string assembly { get; set; }

		protected override void Execute() {
			if ((assemblies == null || assemblies.Length == 0) && string.IsNullOrEmpty(assembly)) {
				throw new InvalidOperationException("Please specify either the 'assembly' or the 'assemblies' property when calling 'xunit'");
			}

			//single assembly takes precedence.
			if (!string.IsNullOrEmpty(assembly)) {
				assemblies = new[] {assembly};
			}

			var args = new List<string>();

			foreach (var asm in assemblies) {
				var xunitArgs = new List<string>(args) {asm};
				Execute(xunitArgs.JoinWith(" "));
			}
		}
	}
}