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

using System;
using NUnit.Framework;

namespace Phantom.Tests {
	using System.IO;
	using System.Linq;

	public static class TestHelper {
		public static void ShouldEqual(this object actual, object expected) {
			Assert.AreEqual(expected, actual);
		}

		public static void ShouldNotBeNull(this object actual) {
			Assert.IsNotNull(actual);
		}

		public static Exception ShouldBeThrownBy(this Type exceptionType, TestDelegate action) {
			return Assert.Throws(exceptionType, action);
		}

		public static void ShouldBeNull(this object actual) {
			Assert.IsNull(actual);
		}

		public static void ShouldBeTrue(this bool actual) {
			Assert.IsTrue(actual);
		}

		public static void AssertOutput(this TextWriter writer, params string[] lines) {
			var output = writer.ToString()
				.Split(new[] { "\r\n" }, StringSplitOptions.None)
				.ToArray();

			for (int i = 0; i < lines.Length; i++) {
				output[i].TrimEnd().ShouldEqual(lines[i]);
			}
		}
	}
}