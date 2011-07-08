namespace Phantom.Core.Builtins {
	using System;
	using System.IO;
	using System.Text;

	public class nuget_push : ExecutableTool<nuget_push> {
		readonly StringBuilder nugetArgs;

		public nuget_push() {
			nugetArgs = new StringBuilder();
		}

		public string packagePath { get; set; }
		public string apiKey { get; set; }
		public bool createOnly { get; set; }
		public string source { get; set; }

		protected override void Execute() {
			if (string.IsNullOrWhiteSpace(packagePath)) {
				throw new InvalidOperationException("Nuget package to push must be specified.");
			}
			if (string.IsNullOrWhiteSpace(apiKey)) {
				throw new InvalidOperationException("Nuget api key is required when pushing a package.");
			}
			nugetArgs.AppendFormat("push \"{0}\" {1}", Path.GetFullPath(packagePath), apiKey);

			if (createOnly) {
				nugetArgs.Append(" -createOnly");
			}

			if (!string.IsNullOrWhiteSpace(source)) {
				nugetArgs.AppendFormat(" -source {0}", source);
			}

			Execute(nugetArgs.ToString());
		}
	}
}
