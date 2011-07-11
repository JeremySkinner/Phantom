namespace Phantom.Core.Builtins {
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;

	public class mstest : ExecutableTool<mstest> {
		readonly List<string> toolPaths;
		readonly StringBuilder args;

		public mstest() {
			args = new StringBuilder();

			toolPaths.Add(@"C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\mstest.exe");
			toolPaths.Add(@"C:\Program Files\Microsoft Visual Studio 10.0\Common7\IDE\mstest.exe");
			toolPaths.Add(@"C:\Program Files (x86)\Microsoft Visual Studio 9.0\Common7\IDE\mstest.exe");
			toolPaths.Add(@"C:\Program Files\Microsoft Visual Studio 9.0\Common7\IDE\mstest.exe");
		}

		public string[] assemblies { get; set; }
		public string assembly { get; set; }
		public string[] options { get; set; }
		public string[] tests { get; set; }

		protected override void Execute() {
			toolPath = GetToolPath();

			if ((assemblies == null || assemblies.Length == 0) && string.IsNullOrEmpty(assembly)) {
				throw new InvalidOperationException("Please specify either 'assembly' or 'assemblies' when calling 'mstest'");
			}

			if (!string.IsNullOrEmpty(assembly)) {
				assemblies = new[] {assembly};
			}

			assemblies = GetAssemblies();

			foreach (var container in assemblies) {
				args.AppendFormat("/testcontainer:\"{0}\"", container);
			}

			if (tests != null || tests.Length != 0) {
				foreach (var test in tests) {
					args.AppendFormat("/test:{0}", test);
				}
			}

			if (options != null || options.Length != 0) {
				args.Append(string.Join(" ", options));
			}
		}

		private string GetToolPath() {
			foreach (var path in toolPaths.Where(File.Exists)) {
				return path;
			}
			throw new InvalidOperationException("MSTest could not be found, please specify in toolPath.");
		}

		private string[] GetAssemblies() {
			var wildcards = new List<string>();
			foreach (var container in assemblies) {
				if(Path.GetFileName(container).Contains('*')) {
					wildcards.Add(container);
				}
			}

			var matching = new List<string>();
			foreach (var wildcard in wildcards) {
				var directory = Path.GetDirectoryName(wildcard);
				var searchPattern = Path.GetFileName(wildcard);

				matching.AddRange(Directory.GetFiles(directory, searchPattern, SearchOption.TopDirectoryOnly));
			}
			matching.AddRange(assemblies.Except(wildcards));

			return matching.Distinct().ToArray();
		}
	}
}
