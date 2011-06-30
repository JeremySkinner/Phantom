lib_path = "../../../../lib"

target packNugetPackage:

  with nuget():
    .toolPath = "${lib_path}/nuget/nuget.exe"
    .nuspecFile = "nuget/phantom.nuspec"
    .basePath = ""
    .outputDirectory = "nuget/nuget_output"
    .version = "1.0.0"