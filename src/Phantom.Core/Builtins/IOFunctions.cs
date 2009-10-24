#region License

// Copyright 2009 Jeremy Skinner (http://www.jeremyskinner.co.uk)
//  
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
//  
// http://www.apache.org/licenses/LICENSE-2.0 
//  
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.
// 
// The latest version of this file can be found at http://github.com/JeremySkinner/Phantom

#endregion

namespace Phantom.Core.Builtins {
	using System;
	using System.Diagnostics;
	using System.IO;
	using System.Runtime.CompilerServices;
	using Boo.Lang;

    [CompilerGlobalScope]
	public sealed class IOFunctions {
		/// <summary>
		/// Executes the specified program with the specified arguments
		/// </summary>
		/// <param name="command">The command to execute</param>
		/// <param name="args">Additional args</param>
		public static void exec(string command, string args) {
		    exec(command, args, new Hash());
		}

        /// <summary>
        /// Executes the specified program with the specified arguments.  You can also
        /// specify the working directory of the command by providing the hash option "WorkingDir"
        /// </summary>
        /// <param name="command">The command to execute</param>
        /// <param name="args">Additional args</param>
        /// <param name="options">A hash of options to set on the process (like WorkingDir)</param>
	    public static void exec(string command, string args, Hash options) {
	        string workingDir = options.ObtainAndRemove("WorkingDir", ".");
            bool ignoreNonZeroExitCode = options.ObtainAndRemove("IgnoreNonZeroExitCode", false);
	        var psi = new ProcessStartInfo(command, args) {
                                                              WorkingDirectory = workingDir,
                                                              UseShellExecute = false,
	                                                          RedirectStandardError = true
	                                                      };
	        var process = Process.Start(psi);
	        process.WaitForExit();
            var exitCode = process.ExitCode;
            
            if (exitCode != 0 && ignoreNonZeroExitCode == false) {
                throw new PhantomException(
                    String.Format("Operation exited with exit code {0}.", exitCode));
            }
	    }

	    public static void exec(string command, Hash options) {
			string commandPrompt = UtilityFunctions.env("COMSPEC");
			string args = string.Format("/C \"{0}\"", command);

			exec(commandPrompt, args, options);
		}

        public static void exec(string command)
        {
            exec(command, new Hash());
        }

		/// <summary>
		/// Copies a file from one location to another.
		/// </summary>
		/// <param name="source">Source file</param>
		/// <param name="destination">Destination</param>
		public static void cp(string source, string destination) {
			Console.WriteLine("Copying '{0}' to '{1}'", source, destination);
			File.Copy(source, destination);
		}

		/// <summary>
		/// Deletes a file or directory.
		/// </summary>
		/// <param name="path"></param>
		public static void rmdir(string path) {
			if (Directory.Exists(path)) {
				Console.WriteLine("Deleting directory '{0}'", path);
				DeleteDirectory(path);
			}
			else if (File.Exists(path)) {
				Console.WriteLine("Deleting file: '{0}'", path);
				DeleteFile(path);
			}
		}

		static void DeleteDirectory(string path) {
			var dirInfo = new DirectoryInfo(path);

			foreach (var dir in dirInfo.GetDirectories()) {
				DeleteDirectory(dir.FullName);
			}

			foreach (var file in dirInfo.GetFiles()) {
				DeleteFile(file.FullName);
			}

			Directory.Delete(path);
		}

		static void DeleteFile(string path) {
			var file = new FileInfo(path);

			if (!file.Exists) return;

			//Ensure that the file attributes are set to Normal so we can do the delete
			if (file.Attributes != FileAttributes.Normal) {
				file.Attributes = FileAttributes.Normal;
			}

			File.Delete(path);
		}


		/// <summary>
		/// Creates a directory if it does not exist.
		/// </summary>
		public static void mkdir(string path) {
			Console.WriteLine("Creating directory '{0}'", path);
			if (!Directory.Exists(path)) {
				Directory.CreateDirectory(path);
			}
		}
	}
}