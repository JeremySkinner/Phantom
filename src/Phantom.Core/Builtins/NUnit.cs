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

	/// <summary>
	/// NUnit integration
	/// </summary>
	public class nunit : ExecutableTool<nunit> {
		public nunit() {
			toolPath = "lib/nunit/nunit-console.exe";
			teamCityArgs = "v2.0 x86 NUnit-2.4.6";
		}

		public string include { get; set; }
		public string exclude { get; set; }
		public bool enableTeamCity { get; set; }
		public string teamCityArgs { get; set; }
		public string[] assemblies { get; set; }
		public string assembly { get; set; }

		string GetTeamCityNunitLancherPath() {
			return UtilityFunctions.env("teamcity.dotnet.nunitlauncher");
		}

		protected override void Execute() {
			if ((assemblies == null || assemblies.Length == 0) && string.IsNullOrEmpty(assembly)) {
				throw new InvalidOperationException("Please specify either the 'assembly' or the 'assemblies' property when calling 'nunit'");
			}

			//single assembly takes precedence.
			if (!string.IsNullOrEmpty(assembly)) {
				assemblies = new[] {assembly};
			}

			var args = new System.Collections.Generic.List<string>();

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
				var nunitArgs = new List<string>(args) {
				                                       	asm
				                                       };

				Execute(nunitArgs.JoinWith(" "));
			}
		}
	}
}