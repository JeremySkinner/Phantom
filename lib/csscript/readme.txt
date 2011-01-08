C# Script execution engine;
Copyright (C) 2004-2009 Oleg Shilo.

-----------------------------------------------------------------------------------------
Licence:
Written by Oleg Shilo (oshilo@gmail.com)

 Copyright (c) 2004-2010 Oleg Shilo
 
 All rights reserved. 

 (The author reserve rights to change the licence for future releases.)

Redistribution and use of this software for commercial and non-commercial purposes in 
source and binary forms are permitted provided that no modifications to the script engine 
and it's framework are made and the following conditions are met:

1. Redistributions of the source code must retain the above copyright notice, 
   this list of conditions and the following disclaimer.
2. Neither the name of an author nor the names of the contributors may be used 
   to endorse or promote products bundled from this software without specific 
   prior written permission.

Redistribution and use of this software in source and binary forms, with modification, 
are permitted provided that all above conditions are met and software is not used or 
sold for profit.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS 
OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY 
AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, 
DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER 
IN CONTRACT, STRICT LIABILITY, OR SORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT 
OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE. 

-----------------------------------------------------------------------------------------
Contact: 
 csscript.support@gmail.com, galos.co@gmail.com 
-----------------------------------------------------------------------------------------
Installation:

 Precondition: .NET runtime must be installed. You can download it from here or from other well known locations:
	http://www.microsoft.com/downloads/details.aspx?FamilyId=333325FD-AE52-4E35-B531-508D977D32A6&displaylang=en
	Despite the fact that you can select earlier versions of .NET as a target .NET Framework version 3.5 is 
    required for CS-Script to function properly.

 To install:
   1. extract content of the cs-script.zip on your HD
   2. run css_config.exe or install.cmd (it will bring the configuration console)
   3. adjust the CS-Script settings in the configuration console according your needs (e.g. enabled debuggers, shell extensions...)
   
 To uininstall:
   1. run css_config.exe
   2. press 'Deactivate' button on the 'General' tab in the configuration console
   OR
   1. run uninstall.cmd
 
 To upgrade:
   No special steps are required. Just do as for normal installation according instructions above.
   
NOTE: 
   - After running css_config.exe from some third-party file navigation utilities (e.g. Total Commander)
   it might be required to restart this utility in order for changes to take effect.
   
-----------------------------------------------------------------------------------------
Installing on "Windows 7 family" OS

If during the installation you have "System.IO.FileNotFoundException: Could not load file or assembly 
'CSScriptLibrary" error this can be due to the new Win& security measures. You may want to "Unblock" all CS-Script 
files you downloaded. This can be done either manually or with Sysinternals Streams.exe utility:  
http://www.rogoff.uk.com/blog/post/How-to-bulk-unblock-files-in-Windows-7-or-Server-2008.aspx

-----------------------------------------------------------------------------------------
Running:
 Script engine can be run in two different modes:
 as a console application (cscscript.exe) and as a WinExe application (cswscript.exe).
 
C# Script execution engine. Version 2.7.3.0.
Copyright (C) 2004-2010 Oleg Shilo.

Usage: cscs.exe <switch 1> <switch 2> <file> [params] [//x]

<switch 1>
 /?    - Display help info.
 /e    - Compile script into console application executable.
 /ew   - Compile script into Windows application executable.
 /c    - Use compiled file (cache file .csc) if found (to improve performance).
 /ca   - Compile script file into assembly (cache file .csc) without execution.
 /cd   - Compile script file into assembly (.dll) without execution.

 /co:<options>
       - Pass compiler options directly to the language compiler
       (e.g. /co:/d:TRACE pass /d:TRACE option to C# compiler
        or /co:/platform:x86 to produce Win32 executable)

 /s    - Print content of sample script file (e.g. cscs.exe /s > sample.cs).
 /ac | /autoclass
       - Automatically generates wrapper class if the script does not define any class of its own:

         using System;
                      
         void Main()
         {
             Console.WriteLine("Hello World!");
         }


<switch 2>
 /nl   - No logo mode: No banner will be shown/printed at execution time.
 /dbg | /d
       - Force compiler to include debug information.
 /l    - 'local'(makes the script directory a 'current directory')
 /verbose 
       - prints runtime information during the script execution (applicable for console clients only)
 /noconfig[:<file>]
       - Do not use default CS-Script config file or use alternative one.
         (e.g. cscs.exe /noconfig sample.cs
              cscs.exe /noconfig:c:\cs-script\css_VB.dat sample.vb)
 /sconfig[:<file>]
       - Use script config file or custom config file as a .NET application configuration file.
This option might be useful for running scripts, which usually cannot be executed without configuration file (e.g. WCF, Remoting).

          (e.g. if /sconfig is used the expected config file name is <script_name>.cs.config or <script_name>.exe.config
           if /sconfig:myApp.config is used the expected config file name is myApp.config
) /r:<assembly 1>:<assembly N>
       - Use explicitly referenced assembly. It is required only for
         rare cases when namespace cannot be resolved into assembly.
         (e.g. cscs.exe /r:myLib.dll myScript.cs).
 /dir:<directory 1>,<directory N>
       - Add path(s) to the assembly probing directory list.
         (e.g. cscs.exe /dir:C:\MyLibraries myScript.cs).

file   - Specifies name of a script file to be run.
params - Specifies optional parameters for a script file to be run.
 //x   - Launch debugger just before starting the script.


**************************************
Script specific syntax
**************************************

Engine directives:
------------------------------------
//css_import <file>[, preserve_main][, rename_namespace(<oldName>, <newName>)];

Alias - //css_imp
There are also another two aliases //css_include and //css_inc. They are equivalents of //css_import <file>, preserve_main
If $this (or $this.name) is specified as part of <file> it will be replaced at execution time with the main script full name (or file name only).

file            - name of a script file to be imported at compile-time.
<preserve_main> - do not rename 'static Main'
oldName         - name of a namespace to be renamed during importing
newName         - new name of a namespace to be renamed during importing

This directive is used to inject one script into another at compile time. Thus code from one script can be exercised in another one.
'Rename' clause can appear in the directive multiple times.
------------------------------------
//css_args arg0[,arg1]..[,argN];

Embedded script arguments. The both script and engine arguments are allowed except "/noconfig" engine command switch.
 Example: //css_args /dbg;
 This directive will always force script engine to execute the script in debug mode.
------------------------------------
//css_reference <file>;

Alias - //css_ref

file	- name of the assembly file to be loaded at run-time.

This directive is used to reference assemblies required at run time.
The assembly must be in GAC, the same folder with the script file or in the 'Script Library' folders (see 'CS-Script settings').
------------------------------------
//css_searchdir <directory>;

Alias - //css_dir

directory - name of the directory to be used for script and assembly probing at run-time.

This directive is used to extend set of search directories (script and assembly probing).
------------------------------------
//css_resource <file>;

Alias - //css_res

file	- name of the resource file (.resources) to be used with the script.

This directive is used to reference resource file for script.
 Example: //css_res Scripting.Form1.resources;
------------------------------------
//css_co <options>;

options - options string.

This directive is used to pass compiler options string directly to the language specific CLR compiler.
 Example: //css_co /d:TRACE pass /d:TRACE option to C# compiler
          //css_co /platform:x86 to produce Win32 executable

------------------------------------
//css_ignore_namespace <namspace>;

Alias - //css_ignore_ns

namspace	- name of the namespace.

This directive is used to prevent CS-Script from resolving the referenced namespace into assembly.
------------------------------------
//css_prescript file([arg0][,arg1]..[,argN])[ignore];
//css_postscript file([arg0][,arg1]..[,argN])[ignore];

Aliases - //css_pre and //css_post

file    - script file (extension is optional)
arg0..N - script string arguments
ignore  - continue execution of the main script in case of error

These directives are used to execute secondary pre- and post-action scripts.
If $this (or $this.name) is specified as arg0..N it will be replaced at execution time with the main script full name (or file name only).
------------------------------------

Any directive has to be written as a single line in order to have no impact on compiling by CLI compliant compiler.
It also must be placed before any namespace or class declaration.

------------------------------------
Example:

 using System;
 //css_prescript com(WScript.Shell, swshell.dll);
 //css_import tick, rename_namespace(CSScript, TickScript);
 //css_reference teechart.lite.dll;
 
 namespace CSScript
 {
   class TickImporter
   {
      static public void Main(string[] args)
      {
         TickScript.Ticker.i_Main(args);
      }
   }
 }
