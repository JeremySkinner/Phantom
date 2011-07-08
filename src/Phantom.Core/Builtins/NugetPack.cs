namespace Phantom.Core.Builtins {
	using System;
	using System.IO;
	using System.Text;

	public class nuget_pack : ExecutableTool<nuget_pack> {
		readonly StringBuilder nugetArgs;

		public nuget_pack() {
			nugetArgs = new StringBuilder();
		}

		public string nuspecFile { get; set; }
		public string basePath { get; set; }
		public string outputDirectory { get; set; }
		public string version { get; set; }
		public bool symbols { get; set; }
		public bool verbose { get; set; }

		protected override void Execute() {
			if (string.IsNullOrWhiteSpace(nuspecFile)) {
				throw new InvalidOperationException("Nuspec file to pack must be specified.");
			}
			nugetArgs.AppendFormat("pack \"{0}\"", Path.GetFullPath(nuspecFile));

			if (string.IsNullOrWhiteSpace(basePath)) {
				basePath = Environment.CurrentDirectory;
			}
			nugetArgs.AppendFormat(" -basePath \"{0}\"", Path.GetFullPath(basePath));

			if (!string.IsNullOrWhiteSpace(outputDirectory)) {
				outputDirectory = Path.GetFullPath(outputDirectory);
				nugetArgs.AppendFormat(" -outputDirectory \"{0}\"", outputDirectory);
				IOFunctions.mkdir(outputDirectory);
			}

			if (!string.IsNullOrWhiteSpace(version)) {
				nugetArgs.AppendFormat(" -version \"{0}\"", version);
			}

			if (symbols) {
				nugetArgs.Append(" -symbols");
			}

			if (verbose) {
				nugetArgs.Append(" -verbose");
			}

			Execute(nugetArgs.ToString());
		}
	}
}
