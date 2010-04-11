namespace Phantom.Tests 
{
    using Core;
    using NUnit.Framework;

    [TestFixture]
    public class PhantomExceptionTests 
    {
        [Test]
        public void execution_exception_prints_error_text_and_exit_code_in_message() 
        {
            var exc = new ExecutionFailedException(220, "error was such and such");
            StringAssert.Contains("Operation exited with exit code 220.", exc.Message);
            StringAssert.Contains("The error message was as follows:\n error was such and such", exc.Message);
            

        }

    }
}