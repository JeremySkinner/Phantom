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
	using System.IO;
	using System.Linq;

	public class ncover_explorer : ExecutableTool<ncover_explorer> {
		public ncover_explorer() {
			reportDirectory = ".";
			minimum = 90;
			failUnderMinimum = false;
			sort = "Name";
		}

		public string project { get; set; }
		public string reportDirectory { get; set; }
		public int minimum { get; set; }
		public bool failUnderMinimum { get; set; }
		public string sort { get; set; }

		protected override void Execute() {
			if (string.IsNullOrEmpty(project)) {
				throw new InvalidOperationException("The 'project' property must be specified.");
			}

			string htmlReport = Path.Combine(reportDirectory, "CoverageReport.html");
			string xmlReport = Path.Combine(reportDirectory, "CoverageReport.xml");

			var files = new FileList(reportDirectory)
				.Include("*.Coverage.xml")
				.Select(x => x.FullName)
				.JoinWith(";");

			if (files.Length == 0) {
				return;
			}

			string args = "{0} /r:ModuleClassFunctionSummary /m:{1} /p:{2} /h:{3} /x:{4} /so:{5}";

			if (failUnderMinimum) {
				args += " /failCombinedMinimum";
			}

			args = string.Format(args, files, minimum, project, htmlReport, xmlReport, sort);

			Execute(args);
		}
	}
}