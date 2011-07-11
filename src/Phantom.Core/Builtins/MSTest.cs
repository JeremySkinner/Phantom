namespace Phantom.Core.Builtins {
	using System.Collections.Generic;

	public class mstest : ExecutableTool<mstest> {
		readonly List<string> toolPaths;

		public mstest() {
			toolPaths.Add(@"C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\mstest.exe");
			toolPaths.Add(@"C:\Program Files\Microsoft Visual Studio 10.0\Common7\IDE\mstest.exe");
			toolPaths.Add(@"C:\Program Files (x86)\Microsoft Visual Studio 9.0\Common7\IDE\mstest.exe");
			toolPaths.Add(@"C:\Program Files\Microsoft Visual Studio 9.0\Common7\IDE\mstest.exe");
		}

		protected override void Execute() {
			if(string.IsNullOrWhiteSpace(toolPath)) {
				
			}
		}
	}
}
