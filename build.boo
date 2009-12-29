solution_file = "Phantom.sln"
configuration = "release"
test_assemblies = "src/Phantom.Tests/bin/${configuration}/Phantom.Tests.dll"

target default, (compile, test, deploy, package):
  pass

target ci, default:
  pass

desc "Compiles the solution"
target compile:
  msbuild(file: solution_file, configuration: configuration)

desc "Executes tests"
target test:
  nunit(assembly: test_assemblies, enableTeamCity: true)

desc "Copies the binaries to the 'build' directory"
target deploy:
  rmdir('build')
	
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