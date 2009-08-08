namespace Spectre.Core {
	using System;
	using System.Collections;
	using System.Collections.Generic;

	public class ScriptModel : IEnumerable<Target> {
		public const string DefaultTargetName = "default";

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
			var target = new Target(name, block, dependencies, this);
			if (targets.ContainsKey(target.Name)) {
				throw new SpectreException(string.Format("A target already exists with the name '{0}'. Target names must be unique.", target.Name));
			}
			targets.Add(target.Name, target);
		}
	}
}