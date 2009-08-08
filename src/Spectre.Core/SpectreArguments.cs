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
	using Boo.Lang.Useful.CommandLine;

	[Serializable]
	public class SpectreArguments : AbstractCommandLine {
		readonly List<string> targetNames = new List<string>();

		[Option("Specifies the build file", LongForm = "file", ShortForm = "f")]
		public string File = "build.boo";

		[Option("Prints the help message", LongForm = "help", ShortForm = "h")]
		public bool Help;


		[Argument]
		public void AddArgument(string argument) {
			targetNames.Add(argument);
		}

		public IEnumerable<string> TargetNames {
			get {
				if(targetNames.Count == 0) {
					yield return "default";
				}
				else {
					foreach(var targetName in targetNames) {
						yield return targetName;
					}
				}
			}	
		}

//		public void Option(string option) {
			
//		}
	}
}