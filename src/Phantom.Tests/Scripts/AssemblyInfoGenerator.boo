target generate:
  rm("TestAssemblyInfo.cs")
  
  with generate_assembly_info():
    .file = "TestAssemblyInfo.cs"
    .namespaces = ["Foo.Bar"]
    .version = "1.0.0.0"
    .fileVersion = "1.0.0.0"
    .title = "Test1"
    .description  = "Test2"
    .copyright = "Test3"
    .customAttributes = { "Foo": "Bar" }
    .comVisible = true
    .companyName = "Test4"
    .productName = "Test5"