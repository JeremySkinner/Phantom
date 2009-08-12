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
// The latest version of this file can be found at http://github.com/JeremySkinner/Phantom

#endregion

namespace Phantom.Core {
	using System;

	public abstract class PhantomBase {
		readonly ScriptModel model = new ScriptModel();

		public void target(string name, Action block) {
			model.AddTarget(name, null, block);
		}

		public void target(string name, string[] dependencies, Action block) {
			model.AddTarget(name, dependencies, block);
		}

		public abstract void Execute();

		public ScriptModel Model {
			get { return model; }
		}

		public void desc(string description) {
			model.SetCurrentDescription(description);
		}
	}
}