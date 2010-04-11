namespace Phantom.Tests {
    using System.IO;
    using NUnit.Framework;
    using Phantom.Core;

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