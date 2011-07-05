using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phantom.Core.Builtins
{
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

		protected override void Execute() {
			if(string.IsNullOrWhiteSpace(nuspecFile)) {
				throw new InvalidOperationException("Please provide a valid nuspecfile.");
			}
			_nugetArgs.AppendFormat("pack \"{0}\" -verbose", nuspecFile);

			if (string.IsNullOrWhiteSpace(basePath)) {
				basePath = Environment.CurrentDirectory;
			}
			_nugetArgs.AppendFormat(" -basePath \"{0}\"", basePath);

			if(!string.IsNullOrWhiteSpace(outputDirectory)) {
				outputDirectory = string.Concat(basePath, @"\", outputDirectory);
				_nugetArgs.AppendFormat(" -outputDirectory \"{0}\"", outputDirectory);

				IOFunctions.mkdir(outputDirectory);
			}

			if(!string.IsNullOrWhiteSpace(version)) {
				_nugetArgs.AppendFormat(" -version \"{0}\"", version);
			}

			Execute(_nugetArgs.ToString());
		}
	}
}
