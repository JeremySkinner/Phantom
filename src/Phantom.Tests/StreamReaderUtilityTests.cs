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
	using System.IO;
	using Core;
	using NUnit.Framework;

	[TestFixture]
	public class StreamReaderUtilityTests {
		[Test]
		public void it_should_return_empty_string_when_stream_is_closed() {
			StreamReader str = new StreamReader(new MemoryStream());
			str.Close();
			string result = str.ReadAllAsString();
			Assert.AreEqual("", result);
		}
	}
}