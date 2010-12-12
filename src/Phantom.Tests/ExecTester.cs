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
	using NUnit.Framework;

	public class ExecTester : ScriptTest {
		[Test]
		public void Executes_correctly_when_exec_returns_exitcode_zero() {
			ScriptFile = "Scripts/Exec.boo";
			Execute("clean_exit", "foo");
			AssertOutput("clean_exit:", "", "foo:", "foo");
		}

		[Test]
		public void Stops_execution_when_exec_call_returns_nonzero_exit_code() {
			ScriptFile = "Scripts/Exec.boo";
			Execute("non_zero_exit", "foo");
			AssertOutput("non_zero_exit:", "Target failed: Operation exited with exit code 99.\nThe error message was as follows:", "");
		}

		[Test]
		public void Continues_execution_when_exec_call_returns_nonzero_exit_and_ignore_option_specified() {
			ScriptFile = "Scripts/Exec.boo";
			Execute("non_zero_exit_ignore", "foo");
			AssertOutput("non_zero_exit_ignore:", "", "foo:", "foo");
		}

		[Test]
		public void Executes_correctly_using_single_string_cmd_exit_0() {
			ScriptFile = "Scripts/Exec.boo";
			Execute("clean_exit_singlestr", "foo");
			AssertOutput("clean_exit_singlestr:", "", "foo:", "foo");
		}

		[Test]
		public void Stops_execution_when_exec_call_returns_nonzero_exit_code_using_single_string_cmd() {
			ScriptFile = "Scripts/Exec.boo";
			Execute("non_zero_exit_singlestr", "foo");
			AssertOutput("non_zero_exit_singlestr:", "Target failed: Operation exited with exit code 99.\nThe error message was as follows:", "");
		}

		[Test]
		public void Continues_execution_when_exec_call_returns_nonzero_exit_and_ignore_option_specified_using_single_str_cmd() {
			ScriptFile = "Scripts/Exec.boo";
			Execute("non_zero_exit_ignore_singlestr", "foo");
			AssertOutput("non_zero_exit_ignore_singlestr:", "", "foo:", "foo");
		}

		[Test]
		public void Phantom_returns_non_zero_exit_code_on_failed_target() {
			ScriptFile = "Scripts/Exec.boo";
			Execute("non_zero_exit");
			Environment.ExitCode.ShouldEqual(1);
		}
	}
}