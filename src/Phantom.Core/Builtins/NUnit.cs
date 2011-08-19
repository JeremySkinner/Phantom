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
	using System.Text;

	/// <summary>
	/// NUnit integration
	/// </summary>
	public class nunit : TestRunner<nunit> {
		public nunit() {
			toolPath = "lib/nunit/nunit-console.exe";
			teamCityArgs = "v2.0 x86 NUnit-2.4.6";
		}

		public string include { get; set; }
		public string exclude { get; set; }
		public bool enableTeamCity { get; set; }
		public string teamCityArgs { get; set; }

		string GetTeamCityNunitLancherPath() {
			return UtilityFunctions.env("teamcity.dotnet.nunitlauncher");
		}

		protected override void Execute() {
			var args = new List<string>();

			if (enableTeamCity) {
				string teamcityLauncherPath = GetTeamCityNunitLancherPath();

				if (!string.IsNullOrEmpty(teamcityLauncherPath)) {
					toolPath = teamcityLauncherPath;
					args.Add(teamCityArgs);
				}
				else {
					enableTeamCity = false;
				}
			}

			if (!string.IsNullOrEmpty(include)) {
				if (enableTeamCity) {
					args.Add(string.Format("/category-include:{0}", exclude));
				}
				else {
					args.Add(string.Format("/include:{0}", include));
				}
			}
			else if (!string.IsNullOrEmpty(exclude)) {
				if (enableTeamCity) {
					args.Add(string.Format("/category-exclude:{0}", exclude));
				}
				else {
					args.Add(string.Format("/exclude:{0}", exclude));
				}
			}

			foreach (var asm in assemblies) {
				args.Add(string.Concat("\"", asm, "\""));
			}

			Execute(args.JoinWith(" "));
		}
	}
}