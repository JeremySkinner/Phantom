import System.IO
#import file from "lib/spectre/BuildUtils.boo"

solution_file = "Spectre.sln"
configuration = "release"
test_assemblies = "src/Spectre.Tests/bin/${configuration}/Spectre.Tests.dll"

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
	exec("xcopy src\\Spectre\\bin\\${configuration}\\*.exe build\\${configuration}")
	exec("xcopy src\\Spectre\\bin\\${configuration}\\*.dll build\\${configuration}")
	exec("xcopy License.txt build\\${configuration}")
	
desc "Creates zip package"
target package:
  zip("build\\${configuration}", 'build\\Spectre.zip')