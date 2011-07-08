using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phantom.Core.Builtins
{
	public class nuget_publish : ExecutableTool<nuget_publish> {
		readonly StringBuilder _nugetArgs;

		public nuget_publish() {
			_nugetArgs = new StringBuilder();
		}

		public string id { get; set; }
		public string version { get; set; }
		public string apiKey { get; set; }
		public string source { get; set; }

		protected override void Execute() {
			if(string.IsNullOrWhiteSpace(id)) {
				throw new InvalidOperationException("Nuget package id is required when publishing a package.");
			}

			if (string.IsNullOrWhiteSpace(version)) {
				throw new InvalidOperationException("Nuget package version is required when publishing a package.");
			}

			if (string.IsNullOrWhiteSpace(apiKey)) {
				throw new InvalidOperationException("Nuget api key is required when publishing a package.");
			}

			_nugetArgs.AppendFormat("publish {0} {1} {2}", id, version, apiKey);

			if(!string.IsNullOrWhiteSpace(source)) {
				_nugetArgs.AppendFormat(" -source {0}", source);
			}

			Execute(_nugetArgs.ToString());
		}
	}
}
