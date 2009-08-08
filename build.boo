#import file from "lib/spectre/BuildUtils.boo"

solution_file = "Spectre.sln"
configuration = "release"
test_assemblies = "src/Spectre.Tests/bin/${configuration}/Spectre.Tests.dll"

target default, (compile, test, package):
  pass
  
desc "Compiles the solution"
target compile:
  msbuild(solution_file, { @configuration: configuration })

desc "Executes tests"
target test:
  nunit(test_assemblies)

desc "Creates zip package"
target package:
  pass
  














