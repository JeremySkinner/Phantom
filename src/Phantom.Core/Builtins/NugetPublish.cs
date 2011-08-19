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
	using System.Text;

	/// <summary>
	/// Publish a nuget package
	/// Based on the NugetPublish rake task from Albacore https://github.com/derickbailey/Albacore/blob/master/lib/albacore/nugetpublish.rb
	/// </summary>
	public class nuget_publish : ExecutableTool<nuget_publish> {
		readonly StringBuilder nugetArgs;

		public nuget_publish() {
			nugetArgs = new StringBuilder();
		}

		public string id { get; set; }
		public string version { get; set; }
		public string apiKey { get; set; }
		public string source { get; set; }

		protected override void Execute() {
			if (string.IsNullOrWhiteSpace(id)) {
				throw new InvalidOperationException("Nuget package id is required when publishing a package.");
			}

			if (string.IsNullOrWhiteSpace(version)) {
				throw new InvalidOperationException("Nuget package version is required when publishing a package.");
			}

			if (string.IsNullOrWhiteSpace(apiKey)) {
				throw new InvalidOperationException("Nuget api key is required when publishing a package.");
			}

			nugetArgs.AppendFormat("publish {0} {1} {2}", id, version, apiKey);

			if (!string.IsNullOrWhiteSpace(source)) {
				nugetArgs.AppendFormat(" -source {0}", source);
			}

			Execute(nugetArgs.ToString());
		}
	}
}
