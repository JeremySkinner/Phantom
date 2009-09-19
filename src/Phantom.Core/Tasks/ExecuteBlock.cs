namespace Phantom.Core.Tasks {
	using System;

	public class ExecuteBlock : Microsoft.Build.Utilities.Task {

		public Action Block { get; set; }

		public override bool Execute() {
			if(Block != null) {
				Block();
			}
			return true;
		}
	}
}