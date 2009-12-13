solution_file = "Phantom.sln"
configuration = "release"
test_assemblies = "src/Phantom.Tests/bin/${configuration}/Phantom.Tests.dll"

target default, (compile, test, deploy, package):
  pass

target ci, default:
  pass

desc "Compiles the solution"
target compile:
  msbuild(solution_file, { @configuration: configuration })

desc "Executes tests"
target test:
  nunit(test_assemblies, { @enableTeamCity: true })

desc "Copies the binaries to the 'build' directory"
target deploy:
  rmdir('build')
  mkdir('build')
  mkdir("build\\${configuration}")
	
  print "Copying to build dir"

  with FileList():
    .Include("src/Phantom/bin/${configuration}/*.{dll,exe}")
    .Include("License.html")
    .Include("readme.txt")
    .ForEach def(file):
      file.CopyToDirectory("build/${configuration}")
	
desc "Creates zip package"
target package:
  zip("build/${configuration}", 'build/Phantom.zip')