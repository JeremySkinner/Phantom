lib_path = "../../../../lib"

target packNugetPackage:
  with nuget_pack():
    .toolPath = "${lib_path}/nuget/nuget.exe"
    .nuspecFile = "nuget/phantom.nuspec"

target packNugetPackageWithOutputDirectory:
  with nuget_pack():
    .toolPath = "${lib_path}/nuget/nuget.exe"
    .nuspecFile = "nuget/phantom.nuspec"
    .outputDirectory = "nuget/nuget_output"

target packNugetPackageWithNewVersion:
  with nuget_pack():
    .toolPath = "${lib_path}/nuget/nuget.exe"
    .nuspecFile = "nuget/phantom.nuspec"
    .outputDirectory = "nuget/nuget_output"
    .version = "2.0.0"

target packNugetPackageWithSymbols:
  with nuget_pack():
    .toolPath = "${lib_path}/nuget/nuget.exe"
    .nuspecFile = "nuget/phantom.nuspec"
    .outputDirectory = "nuget/nuget_output"
    .symbols = true

target packNugetPackageNoNuspec:
  with nuget_pack():
    .toolPath = "${lib_path}/nuget/nuget.exe"