Phantom is a .NET build system written in C# and Boo. 

Command line options:

phantom [-f filename] [-t] [-h] targets

 -f  -file:file    Specifies the build file
 -h  -help[+-]     Prints the help message
 -t  -targets[+-]  Shows all the targets in the specified build file
 
Running "phantom.exe" with no arguments will look for a file named "build.boo" in the current directory
and execute a target named "default".
 
You can define targets in your build.boo using the "target" method. 
The first argument is the name of the target:
 
target default:
  print "Default target"
  
You can also specify dependencies using an array of other target names:
 
target default, (compile, test):
  print "The Compile and Test targets will execute before default"
  
You can specify a description for a target by calling the "desc" method:

desc "The default target"
target default:
  print "executing..."
  
These descriptions can be viewed by running phantom -t, eg:

Targets in build.boo:
compile          Compiles the solution
default
package          Creates zip package
test             Executes tests

You can use MSBuild to compile .net projects using the "msbuild" method:

target compile:
  msbuild("path/to/MyProject.sln", { @configuration: "release" })

You can execute nunit tests by calling the "nunit" method:

test_assemblies = ("path/to/TestAssembly.dll", "path/to/AnotherTEstAssembly.dll")
target test:
  nunit(test_assemblies)
  
By default, Phantom will look for NUnit.exe in the path "lib/nunit/nunit.console.exe". 
This can be overriden by specifying an optional "path" variable:

target test:
  nunit(test_assemblies, { @path: "path/to/nunit.console.exe" })

Arbitrary programs can be executed by calling "exec":

target runNotepad:
  exec("notepad.exe", "")

FileLists can be used to find all paths matching a pattern:

FileList.Create def(fl):
	fl.Include("path/to/compilation/directory/*.{dll,exe}")
	fl.Include("License.txt")
	fl.ForEach def(file):
		file.CopyTo("build")

Directories can be zipped using 'zip':

zip("path/to/dir", "dir.zip")
  
This project is licensed under the Microsoft Public License (http://www.microsoft.com/opensource/licenses.mspx).
This project uses some code from the IronRuby projet (http://www.ironruby.net/) which is licensed under the Microsoft Public License.