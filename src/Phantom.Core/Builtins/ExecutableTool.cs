namespace Phantom.Core.Builtins {
	using System;
	using System.IO;
	using Language;

	public abstract class ExecutableTool<T> : IRunnable<T> where T : ExecutableTool<T> {
		public string toolPath { get; set; }

		protected abstract void Execute();

		protected void Execute(string args) {
            if(! File.Exists(toolPath)) {
				throw new FileNotFoundException(string.Format("Could not execute the file '{0}' as it does not exist.", toolPath));
            }

			IOFunctions.exec(toolPath, args);
		}

		public T Run() {
			if(toolPath == null) {
				throw new InvalidOperationException("toolPath must be specified.");
			}

			Execute();
			return (T)this;
		}
	}
}