#region License

// Copyright Jeremy Skinner (http://www.jeremyskinner.co.uk) and Contributors
// 
// Licensed under the Microsoft Public License. You may
// obtain a copy of the license at:
// 
// http://www.microsoft.com/opensource/licenses.mspx
// 
// By using this source code in any fashion, you are agreeing
// to be bound by the terms of the Microsoft Public License.
// 
// You must not remove this notice, or any other, from this software.

#endregion

namespace Phantom.Core.Builtins {
	using System;
	using System.IO;
	using Language;

	public abstract class ExecutableTool<T> : IRunnable<T> where T : ExecutableTool<T> {
		public string toolPath { get; set; }

		protected abstract void Execute();

		protected void Execute(string args) {
			if (! File.Exists(toolPath)) {
				throw new FileNotFoundException(string.Format("Could not execute the file '{0}' as it does not exist.", toolPath));
			}

			IOFunctions.exec(toolPath, args);
		}

		public T Run() {
			if (toolPath == null) {
				throw new InvalidOperationException("toolPath must be specified.");
			}

			Execute();
			return (T) this;
		}
	}
}