lib_path = "../../../../lib"

target publishNugetPackageWithoutId:
  with nuget_publish():
    .toolPath = "${lib_path}/nuget/nuget.exe"
    .version = "1.0.1"
    .apiKey = "asdf-asdf-asdf-asdf"
    .source = "http://localhost/"

target publishNugetPackageWithoutVersion:
  with nuget_publish():
    .toolPath = "${lib_path}/nuget/nuget.exe"
    .id = "asdf-asdf-asdf-asdf"
    .apiKey = "asdf-asdf-asdf-asdf"
    .source = "http://localhost/"

target publishNugetPackageWithoutApiKey:
  with nuget_publish():
    .toolPath = "${lib_path}/nuget/nuget.exe"
    .id = "asdf-asdf-asdf-asdf"
    .version = "1.0.1"
    .source = "http://localhost/"