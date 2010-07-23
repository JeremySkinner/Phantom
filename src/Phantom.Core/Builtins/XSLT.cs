namespace Phantom.Core.Builtins {
	using System;
	using System.IO;
	using System.Xml;
	using System.Xml.XPath;
	using System.Xml.Xsl;
	using Language;

	public class xslt : IRunnable<xslt> {
		public string data { get; set; }
		public string transform { get; set; }
		public string output { get; set; }

		public xslt Run() {
			if(string.IsNullOrEmpty(data)) {
				throw new InvalidOperationException("'data' parameter must be specified. This should point to the path of the xml file.");
			}

			if(string.IsNullOrEmpty(transform)) {
				throw new InvalidOperationException("'transform' parameter must be specified. This should point to the path of the XSLT file.");
			}

			if(string.IsNullOrEmpty(output)) {
				throw new InvalidOperationException("'output' parameter must be specified.");
			}

			if(! File.Exists(data)) {
				throw new FileNotFoundException(string.Format("data file '{0}' does not exist.", data));
			}

			if(!File.Exists(transform)) {
				throw new FileNotFoundException(string.Format("transform file '{0}' does not exist", transform));
			}

			var xmlDoc = new XPathDocument(data);
			var compiledTransform = new XslCompiledTransform();
			compiledTransform.Load(transform);

			using (var writer = new XmlTextWriter(output, null)) {
				compiledTransform.Transform(xmlDoc, null, writer);
			}
			return this;
		}
	}
}