namespace Phantom.Tests {
    using System;
    using System.IO;
    using Core;
    using NUnit.Framework;

    [TestFixture]
    public class ExecTester {
        StringWriter writer;
        BuildRunner runner;

        [SetUp]
        public void Setup()
        {
            writer = new StringWriter();
            Console.SetOut(writer);
            runner = new BuildRunner();
        }

        [Test]
        public void Executes_correctly_when_exec_returns_exitcode_zero() {
            var options = new PhantomOptions() { File = "Scripts\\Exec.boo" };
            options.AddTarget("clean_exit");
            options.AddTarget("foo");
            runner.Execute(options);
            writer.AssertOutput("clean_exit:", "", "foo:", "foo");
        }

        [Test]
        public void Stops_execution_when_exec_call_returns_nonzero_exit_code() {
            var options = new PhantomOptions() { File = "Scripts\\Exec.boo" };
            options.AddTarget("non_zero_exit");
            options.AddTarget("foo");
            runner.Execute(options);
            writer.AssertOutput("non_zero_exit:", "Target failed: Operation exited with exit code 99.", "");
        }

        [Test]
        public void Continues_execution_when_exec_call_returns_nonzero_exit_and_ignore_option_specified() {
            var options = new PhantomOptions() { File = "Scripts\\Exec.boo" };
            options.AddTarget("non_zero_exit_ignore");
            options.AddTarget("foo");
            runner.Execute(options);
            writer.AssertOutput("non_zero_exit_ignore:", "", "foo:", "foo");
        }

        [Test]
        public void Executes_correctly_using_single_string_cmd_exit_0() {
            var options = new PhantomOptions() { File = "Scripts\\Exec.boo" };
            options.AddTarget("clean_exit_singlestr");
            options.AddTarget("foo");
            runner.Execute(options);
            writer.AssertOutput("clean_exit_singlestr:", "", "foo:", "foo");
        }

        [Test]
        public void Stops_execution_when_exec_call_returns_nonzero_exit_code_using_single_string_cmd()
        {
            var options = new PhantomOptions() { File = "Scripts\\Exec.boo" };
            options.AddTarget("non_zero_exit_singlestr");
            options.AddTarget("foo");
            runner.Execute(options);
            writer.AssertOutput("non_zero_exit_singlestr:", "Target failed: Operation exited with exit code 99.", "");
        }

        [Test]
        public void Continues_execution_when_exec_call_returns_nonzero_exit_and_ignore_option_specified_using_single_str_cmd()
        {
            var options = new PhantomOptions() { File = "Scripts\\Exec.boo" };
            options.AddTarget("non_zero_exit_ignore_singlestr");
            options.AddTarget("foo");
            runner.Execute(options);
            writer.AssertOutput("non_zero_exit_ignore_singlestr:", "", "foo:", "foo");
        }
    }
}