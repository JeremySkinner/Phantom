namespace Phantom.Tests {
	using System.IO;
	using NUnit.Framework;

	[TestFixture]
	public class XsltTests : ScriptTest {
		[Test]
		public void Generates_html_from_xml() {
			ScriptFile = "Scripts/Xslt.boo";
			Execute("default");

			string text = File.ReadAllText("XmlReport.html");
			text.ShouldEqual("<div>bar</div><div>baz</div>");
		}
	}
}