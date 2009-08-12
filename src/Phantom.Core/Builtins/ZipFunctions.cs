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

namespace Phantom.Core.Builtins {
	using System;
	using System.Runtime.CompilerServices;
	using Ionic.Zip;

	[CompilerGlobalScope]
	public sealed class ZipFunctions {
		public static void zip(string directory, string zipFileName) {
			Console.WriteLine("Zipping '{0}' to '{1}'", directory, zipFileName);
			using (var zip = new ZipFile()) {
				zip.AddDirectory(directory);
				zip.Save(zipFileName);
			}
		}
	}
}