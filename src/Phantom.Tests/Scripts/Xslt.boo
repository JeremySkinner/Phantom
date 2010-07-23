target default:
  rm("XmlReport.html")
  xslt(data: "XmlData.xml", transform: "XmlTransform.xslt", output: "XmlReport.html")
  