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

namespace Phantom.Core {
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class Target {
		readonly Action block;
		readonly ScriptModel parentScript;
		readonly string[] dependencyNames;


		public Target(string name, Action block, string[] dependencies, string description, ScriptModel parentScript) {
			Name = name;
			Description = description;
			this.block = block;
			this.parentScript = parentScript;
			dependencyNames = dependencies ?? new string[0];
		}

		public string Name { get; private set; }
		public string Description { get; private set; }

		public IEnumerable<Target> GetExecutionSequence() {
			var executionSequence = new List<Target>();
			var parsedSequence = new List<Target>();

			PopulateExecutionSequence(executionSequence, parsedSequence);

			return executionSequence.AsEnumerable().Reverse();
		}


		void PopulateExecutionSequence(ICollection<Target> executionSequence, ICollection<Target> parsedSequence) {
			if (parsedSequence.Contains(this)) {
				return;
			}

			if (executionSequence.Contains(this)) {
				throw new RecursiveDependencyException(Name);
			}

			executionSequence.Add(this);

			foreach (var dependencyName in dependencyNames.Reverse()) {
				var dependency = EnsureTargetExists(dependencyName);

				dependency.PopulateExecutionSequence(executionSequence, parsedSequence);
			}

			parsedSequence.Add(this);
		}

		Target EnsureTargetExists(string dependencyName) {
			var dependency = parentScript.GetTarget(dependencyName);

			if (dependency == null) {
				throw new TargetNotFoundException(Name, dependencyName);
			}

			return dependency;
		}

		public void Execute() {
			if (block != null) {
				block();
			}
		}
	}
}