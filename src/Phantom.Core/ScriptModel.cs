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
	using System.Collections;
	using System.Collections.Generic;

	public class ScriptModel : IEnumerable<Target> {
		public const string DefaultTargetName = "default";
		string currentDescription;

		readonly Dictionary<string, Target> targets = new Dictionary<string, Target>();

		public Target DefaultTarget {
			get { return GetTarget(DefaultTargetName); }
		}

		public Target GetTarget(string name) {
			return targets.ValueOrDefault(name);
		}

		public IEnumerator<Target> GetEnumerator() {
			return targets.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		public void AddTarget(string name, string[] dependencies, Action block) {
			var target = new Target(name, block, dependencies, currentDescription, this);
			currentDescription = null;
			if (targets.ContainsKey(target.Name)) {
				throw new TargetAlreadyExistsException(target.Name);
			}
			targets.Add(target.Name, target);
		}

		public void ExecuteTargets(params string[] targetNames) {
			var targetsToExecute = new List<Target>();

			foreach (var targetName in targetNames) {
				var rootTarget = GetTarget(targetName);

				if (rootTarget == null) {
					throw new TargetNotFoundException(targetName);
				}

				foreach (var targetInSequence in rootTarget.GetExecutionSequence()) {
					if (! targetsToExecute.Contains(targetInSequence)) {
						targetsToExecute.Add(targetInSequence);
					}
				}
			}

			try {
				foreach (var target in targetsToExecute) {
					Console.WriteLine(target.Name + ":");
					target.Execute();
					Console.WriteLine();
				}
			}
			catch (PhantomException e) {
				Console.WriteLine(
					string.Format("Target failed: {0}", e.Message));
				Environment.ExitCode = 1;
			}
		}

		public void SetCurrentDescription(string description) {
			currentDescription = description;
		}
	}
}