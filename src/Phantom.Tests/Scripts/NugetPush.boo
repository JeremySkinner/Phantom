lib_path = "../../../../lib"

target pushNugetPackageWithoutPackage:
  with nuget_push():
    .toolPath = "${lib_path}/nuget/nuget.exe"
    .apiKey = "asdf-asdf-asdf-asdf"
    .source = "http://localhost/"
    .createOnly = true

target pushNugetPackageWithoutApiKey:
  with nuget_push():
    .toolPath = "${lib_path}/nuget/nuget.exe"
    .packagePath = "../nuget/nuget_output/Phantom.1.0.nupkg"
    .source = "http://localhost/"