namespace Spectre.Core {
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
				throw new SpectreException(string.Format("A target already exists with the name '{0}'. Target names must be unique.", target.Name));
			}
			targets.Add(target.Name, target);
		}

		public void ExecuteTargets(params string[] targetNames) {
			var targetsToExecute = new List<Target>();

			foreach(var targetName in targetNames) {
				var rootTarget = GetTarget(targetName);

				if (rootTarget == null) {
					throw new SpectreException("Target '{0}' does not exist.");
				}

				foreach(var targetInSequence in rootTarget.GetExecutionSequence()) {
					if(! targetsToExecute.Contains(targetInSequence)) {
						targetsToExecute.Add(targetInSequence);
					}
				}
			}

			foreach (var target in targetsToExecute) {
				target.Execute();
			}
		}

		public void SetCurrentDescription(string description) {
			this.currentDescription = description;
		}
	}
}