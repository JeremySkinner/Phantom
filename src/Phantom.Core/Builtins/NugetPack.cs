namespace Phantom.Core.Builtins
{
	using System;
	using System.IO;
	using System.Text;

	public class nuget_pack : ExecutableTool<nuget_pack> {
		readonly StringBuilder _nugetArgs;

		public nuget_pack() {
			_nugetArgs = new StringBuilder();
		}

		public string nuspecFile { get; set; }
		public string basePath { get; set; }
		public string outputDirectory { get; set; }
		public string version { get; set; }
		public bool symbols { get; set; }
		public bool verbose { get; set; }

		protected override void Execute() {
			if (string.IsNullOrWhiteSpace(nuspecFile)) {
				throw new InvalidOperationException("Nuspec must be specified.");
			}
			_nugetArgs.AppendFormat("pack \"{0}\"", Path.GetFullPath(nuspecFile));

			if(string.IsNullOrWhiteSpace(basePath)) {
				basePath = Environment.CurrentDirectory;
			}
			_nugetArgs.AppendFormat(" -basePath \"{0}\"", Path.GetFullPath(basePath));

			if(!string.IsNullOrWhiteSpace(outputDirectory)) {
				outputDirectory = Path.GetFullPath(outputDirectory);
				_nugetArgs.AppendFormat(" -outputDirectory \"{0}\"", outputDirectory);
				IOFunctions.mkdir(outputDirectory);
			}

			if(!string.IsNullOrWhiteSpace(version)) {
				_nugetArgs.AppendFormat(" -version \"{0}\"", version);
			}

			if(symbols) {
				_nugetArgs.Append(" -symbols");
			}

			if(verbose) {
				_nugetArgs.Append(" -verbose");
			}

			Execute(_nugetArgs.ToString());
		}
	}
}
