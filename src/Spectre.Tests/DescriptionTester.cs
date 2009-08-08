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

namespace Spectre.Tests {
	using System;
	using System.IO;
	using System.Linq;
	using Core;
	using NUnit.Framework;

	[TestFixture]
	public class DescriptionTester {
		BuildRunner runner;
		TextWriter writer;

		[SetUp]
		public void Setup() {
			runner = new BuildRunner();
			writer = new StringWriter();
			Console.SetOut(writer);
		}

		[Test]
		public void Outputs_description() {
			runner.OutputTargets(new SpectreOptions {File = "Scripts\\Descriptions.boo"});
			writer.AssertOutput(
				"Targets in Scripts\\Descriptions.boo:",
				"compile          Compiles",
				"default          The default"
				);
		}

		[Test]
		public void Truncates_long_Descriptions() {
			runner.OutputTargets(new SpectreOptions {File = "Scripts\\LongDescription.boo"});
			writer.AssertOutput(
				"Targets in Scripts\\LongDescription.boo:",
				"default          The quick brown fox jumped over the lazy dog th..."
				);
		}

	}
}