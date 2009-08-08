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
	using System.Linq;
	using Rhino.DSL;

	public class BuildRunner {
		static readonly DslFactory dslFactory;

		static BuildRunner() {
			dslFactory = new DslFactory();
			dslFactory.Register<SpectreBase>(new SpectreDslEngine());
		}

		public ScriptModel GenerateBuildScript(string path) {
			var script = dslFactory.Create<SpectreBase>(path);
			script.Execute();
			return script.Model;
		}

		public void Execute(SpectreOptions options) {
			var script = GenerateBuildScript(options.File);
			script.ExecuteTargets(options.TargetNames.ToArray());
		}

		public void OutputTargets(SpectreOptions options) {
			Console.WriteLine("Targets in {0}: ", options.File);
			var script = GenerateBuildScript(options.File);
			var allTargets = script.OrderBy(x => x.Name).ToList();

			int maxTargetLength = allTargets.Max(x => x.Name.Length);

			foreach(var target in script.OrderBy(x => x.Name)) {
				string name = target.Name.PadRight(maxTargetLength + 10, ' ');
				string description = target.Description ?? string.Empty;

				if(description.Length > 47) {
					description = description.Substring(0, 47) + "...";
				}

				Console.WriteLine(name + description);
			}
		}
	}
}