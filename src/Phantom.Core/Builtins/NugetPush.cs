using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phantom.Core.Builtins
{
	using System.IO;

	public class nuget_push : ExecutableTool<nuget_push> {
		readonly StringBuilder _nugetArgs;

		public nuget_push() {
			_nugetArgs = new StringBuilder();
		}

		public string packagePath { get; set; }
		public string apiKey { get; set; }
		public bool createOnly { get; set; }
		public string source { get; set; }

		protected override void Execute() {
			if(string.IsNullOrWhiteSpace(packagePath)) {
				throw new InvalidOperationException("Nuget package to push must be specified.");
			}
			if(string.IsNullOrWhiteSpace(apiKey)) {
				throw new InvalidOperationException("Nuget api key is required when pushing a package.");
			}
			_nugetArgs.AppendFormat("push \"{0}\" {1}", Path.GetFullPath(packagePath), apiKey);

			if(createOnly) {
				_nugetArgs.Append(" -createOnly");
			}

			if(!string.IsNullOrWhiteSpace(source)) {
				_nugetArgs.AppendFormat(" -source {0}", source);
			}

			Execute(_nugetArgs.ToString());
		}
	}
}
