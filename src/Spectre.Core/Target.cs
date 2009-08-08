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
	using System.Collections.Generic;

	public class Target {
		readonly Action block;
		readonly ScriptModel parentScript;
		readonly string[] dependencyNames;

		public Target(string name, Action block, string[] dependencies, ScriptModel parentScript) {
			Name = name;
			this.block = block;
			this.parentScript = parentScript;
			dependencyNames = dependencies ?? new string[0];
		}

		public string Name { get; private set; }

		public IEnumerable<Target> GetExecutionSequence() {
			foreach (var dependency in GetDependencies()) {
				yield return dependency;
			}
			yield return this;
		}

		IEnumerable<Target> GetDependencies() {
			foreach (var dependencyName in dependencyNames) {
				var dependency = parentScript.GetTarget(dependencyName);

				if (dependency == null) {
					throw new SpectreException(string.Format("Target '{0}' depenends upon a target named '{1}' but it does not exist.",
					                                         Name, dependencyName));
				}

				foreach (var childDependency in dependency.GetDependencies()) {
					yield return childDependency;
				}
				yield return dependency;
			}
		}
	}
}