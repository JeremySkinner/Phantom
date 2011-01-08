#region License

// Copyright Jeremy Skinner (http://www.jeremyskinner.co.uk) and Contributors
// 
// Licensed under the Microsoft Public License. You may
// obtain a copy of the license at:
// 
// http://www.microsoft.com/opensource/licenses.mspx
// 
// By using this source code in any fashion, you are agreeing
// to be bound by the terms of the Microsoft Public License.
// 
// You must not remove this notice, or any other, from this software.

#endregion

namespace Phantom.Tests {
	using System;
	using System.IO;
	using System.Linq;
	using NUnit.Framework;

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
				.Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.None)
				.ToArray();

			for (int i = 0; i < lines.Length; i++) {
				output[i].TrimEnd().ShouldEqual(lines[i]);
			}
		}
	}
}