solution_file = "Phantom.sln"
configuration = "release"
test_assemblies = "src/Phantom.Tests/bin/${configuration}/Phantom.Tests.dll"

target default, (init, compile, test, deploy, package):
  pass

target ci, (init, compile, coverage, deploy, package):
  pass

target init:
  rmdir("build")

desc "Compiles the solution"
target compile:
  msbuild(file: solution_file, configuration: configuration)

desc "Executes tests"
target test:
  nunit(assembly: test_assemblies, enableTeamCity: true)

desc "Copies the binaries to the 'build' directory"
target deploy:
  print "Copying to build dir"

  with FileList("src/Phantom/bin/${configuration}"):
    .Include("*.{dll,exe}")
    .ForEach def(file):
      file.CopyToDirectory("build/${configuration}/Phantom")
      
  with FileList():
    .Include("License.html")
    .Include("readme.txt")
    .ForEach def(file):
      file.CopyToDirectory("build/${configuration}")

  with FileList("src/Phantom.Integration.Nant/bin/${configuration}"):
    .Include("NAnt.Core.dll")
    .Include("Phantom.Integration.Nant.*")
    .Include("*.txt")
    .ForEach def(file):
      file.CopyToDirectory("build/${configuration}/NantIntegration")

desc "Creates zip package"
target package:
  zip("build/${configuration}", 'build/Phantom.zip')

desc "Runs code coverage with ncover (only runs on build server)"
target coverage:
  ncover_path = "C:/Program Files (x86)/ncover"
  app_assemblies = ("Phantom.Core",)
  teamcity_launcher = env("teamcity.dotnet.nunitlauncher")
  
  with ncover():
    .toolPath = "${ncover_path}/NCover.console.exe"
    .reportDirectory = "build/Coverage"
    .workingDirectory = "src/Phantom.Tests/bin/${configuration}"
    .applicationAssemblies = app_assemblies
    .program = "${teamcity_launcher} v2.0 x86 NUnit-2.4.6"
    .testAssembly = "Phantom.Tests.dll"
    .excludeAttributes = "Phantom.Core.ExcludeFromCoverageAttribute;System.Runtime.CompilerServices.CompilerGeneratedAttribute"
  
  with ncover_explorer():
    .toolPath = "${ncover_path}/NCoverExplorer.console.exe"
    .project = "Phantom"
    .reportDirectory = "build/Coverage"