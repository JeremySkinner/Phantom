import System.IO

solution_file = "Phantom.sln"
configuration = "release"
test_assemblies = "src/Phantom.Tests/bin/${configuration}/Phantom.Tests.dll"

target default, (compile, test, deploy, package):
  pass
  
desc "Compiles the solution"
target compile:
  msbuild(solution_file, { @configuration: configuration })

desc "Executes tests"
target test:
  nunit(test_assemblies)

desc "Copies the binaries to the 'build' directory"
target deploy:
	rmdir('build')
	mkdir('build')
	mkdir("build\\${configuration}")
	
	print "Copying to build dir"
	fl = FileList()
	fl.Include("src/Phantom/bin/${configuration}/*.{dll,exe}")
	fl.Include("License.html")
	fl.ForEach def(file):
		file.CopyToDir("build/${configuration}")
	
desc "Creates zip package"
target package:
  zip("build/${configuration}", 'build/Phantom.zip')