using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phantom.Core.Builtins
{
	public class nuget : ExecutableTool<nuget>
	{
		public string nuspecFile { get; set; }
		public string basePath { get; set; }
		public string outputDirectory { get; set; }
		public string version { get; set; }
		//public bool publishAfterPack { get; set; }
		//public string apiKey { get; set; }
		//public string publishToSource { get; set; }

		protected override void Execute() {
			throw new NotImplementedException();
		}
	}
}
