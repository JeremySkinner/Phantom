#region License

// Copyright 2009 Jeremy Skinner (http://www.jeremyskinner.co.uk)
//  
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
//  
// http://www.apache.org/licenses/LICENSE-2.0 
//  
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.
// 
// The latest version of this file can be found at http://github.com/JeremySkinner/Spectre

#endregion

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
				throw new SpectreException(string.Format(
				                           	"A target already exists with the name '{0}'. Target names must be unique.", target.Name));
			}
			targets.Add(target.Name, target);
		}

		public void ExecuteTargets(params string[] targetNames) {
			var targetsToExecute = new List<Target>();

			foreach (var targetName in targetNames) {
				var rootTarget = GetTarget(targetName);

				if (rootTarget == null) {
					throw new SpectreException("Target '{0}' does not exist.");
				}

				foreach (var targetInSequence in rootTarget.GetExecutionSequence()) {
					if (! targetsToExecute.Contains(targetInSequence)) {
						targetsToExecute.Add(targetInSequence);
					}
				}
			}

			foreach (var target in targetsToExecute) {
				target.Execute();
			}
		}

		public void SetCurrentDescription(string description) {
			currentDescription = description;
		}
	}
}