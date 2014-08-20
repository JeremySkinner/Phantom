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
			var temporaryMarks = new List<Target>();
			var permanentMarks = new List<Target>();

			PopulateExecutionSequence(executionSequence, temporaryMarks, permanentMarks);

			return executionSequence.AsEnumerable();
		}


		void PopulateExecutionSequence(List<Target> executionSequence, ICollection<Target> temporaryMarks, ICollection<Target> permanentMarks) {
			if (temporaryMarks.Contains(this)) {
				throw new RecursiveDependencyException(Name);
			}

			if (permanentMarks.Contains(this)) {
				return;
			}

			temporaryMarks.Add(this);

			foreach (var dependencyName in dependencyNames) {
				var dependency = EnsureTargetExists(dependencyName);

				dependency.PopulateExecutionSequence(executionSequence, temporaryMarks, permanentMarks);
			}

			permanentMarks.Add(this);
			temporaryMarks.Remove(this);
			executionSequence.Add(this);
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