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
	using Core;
	using NUnit.Framework;

	[TestFixture]
	public class PhantomExceptionTests {
		[Test]
		public void execution_exception_prints_error_text_and_exit_code_in_message() {
			var exc = new ExecutionFailedException(220, "error was such and such");
			StringAssert.Contains("Operation exited with exit code 220.", exc.Message);
			StringAssert.Contains("The error message was as follows:\n error was such and such", exc.Message);
		}
	}
}