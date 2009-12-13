Phantom is a .NET build system written in C# and Boo.

For discussion, please use the mailing list.
http://groups.google.com/group/phantom-discuss

For complete documentation see the Phantom wiki.
http://wiki.github.com/JeremySkinner/Phantom

This project is licensed under the Microsoft Public License. 
http://www.microsoft.com/opensource/licenses.mspx

Example:

desc "Compiles the solution"
target compile:
  msbuild("MySolution.sln", { @configuration: configuration })

desc "Executes tests"
target test:
  nunit("path/to/TestAssembly.dll")

desc "Copies the binaries to the 'build' directory"
target deploy:
	rmdir('build')
	mkdir('build')
	
	with FileList():
		.Include("src/MyApp/bin/release/*.{dll,exe}")
		.Include("readme.txt")
		.ForEach def(file):
			file.CopyToDir("build")
	
desc "Creates zip package"
target package:
  zip("build", 'build/MyApp.zip')
