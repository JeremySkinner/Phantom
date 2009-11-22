#region License

// Copyright Jeremy Skinner (http://www.jeremyskinner.co.uk)
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
// 
// The latest version of this file can be found at http://github.com/JeremySkinner/Phantom

#endregion

namespace Phantom.Core {
	using System;

	public abstract class PhantomException : ApplicationException {
		public PhantomException(string message) : base(message) {
		}
	}

	public class RecursiveDependencyException : PhantomException {
		public RecursiveDependencyException(string targetName) 
			: base(string.Format("Detected recursive dependency for target '{0}'", targetName)) {
			TargetName = targetName;
		}

		public string TargetName { get; private set; }
	}

	public class TargetAlreadyExistsException : PhantomException {
		public TargetAlreadyExistsException(string name)
			: base(string.Format("A target already exists with the name '{0}'. Target names must be unique.",name )) {
			TargetName = name;			
		}
		public string TargetName { get; private set; }

	}

	public class TargetNotFoundException : PhantomException {
		public TargetNotFoundException(string parentTarget, string dependency)
			: base(string.Format("Target '{0}' depenends upon a target named '{1}' but it does not exist.", parentTarget, dependency)) {
			TargetName = dependency;
		}

		public TargetNotFoundException(string name)
			: base(string.Format("Target '{0}' does not exist.", name)) {
			TargetName = name;
		}

		public string TargetName { get; private set; }
	}

	public class ScriptParsingException : PhantomException {
		public ScriptParsingException(string message) : base(message) {
		}
	}

	public class ExecutionFailedException : PhantomException {
		public ExecutionFailedException(int exitCode)
			: base(string.Format("Operation exited with exit code {0}.", exitCode)) {
		}
	}
}