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
			var executionSequence = new List<Target>() {this};
			PopulateExecutionSequence(executionSequence);
			return executionSequence.AsEnumerable().Reverse();
		}

		void PopulateExecutionSequence(ICollection<Target> sequence) {
			foreach (var dependencyName in dependencyNames.Reverse()) {
				var dependency = parentScript.GetTarget(dependencyName);

				if (dependency == null) {
					throw new PhantomException(string.Format("Target '{0}' depenends upon a target named '{1}' but it does not exist.",
					                                         Name, dependencyName));
				}

				if (sequence.Contains(dependency)) {
					throw new PhantomException(string.Format("Detected recursive dependency for target '{0}'", dependency.Name));
				}

				sequence.Add(dependency);
				dependency.PopulateExecutionSequence(sequence);
			}
		}


		public void Execute() {
			if (block != null) {
				block();
			}
		}
	}
}