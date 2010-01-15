#region License

// Copyright Jeremy Skinner (http://www.jeremyskinner.co.uk) and Contributors
// 
// Licensed under the Microsoft Public License. You may
// obtain a copy of the license at:
// 
// http://www.microsoft.com/opensource/licenses.mspx
// 
// By using this source code in any fashion, you are agreeing
// to be bound by the terms of the Microsoft Public License.
// 
// You must not remove this notice, or any other, from this software.

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
        ///   Executes the specified program with the specified arguments
        /// </summary>
        /// <param name="command">
        ///   The command to execute
        /// </param>
        /// <param name="args">Additional args</param>
        public static void exec(string command, string args) {
            exec(command, args, new Hash());
        }

        /// <summary>
        ///   Executes the specified program with the specified arguments.  You can also
        ///   specify the working directory of the command by providing the hash option "WorkingDir"
        /// </summary>
        /// <param name="command">
        ///   The command to execute
        /// </param>
        /// <param name="args">Additional args</param>
        /// <param name="options">
        ///   A hash of options to set on the process (like WorkingDir)
        /// </param>
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
                throw new ExecutionFailedException(exitCode);
            }
        }

        public static void exec(string command, Hash options) {
            string commandPrompt = UtilityFunctions.env("COMSPEC");
            string args = string.Format("/C \"{0}\"", command);

            exec(commandPrompt, args, options);
        }

        public static void exec(string command) {
            exec(command, new Hash());
        }

        /// <summary>
        ///   Copies a file from one location to another.
        /// </summary>
        /// <param name="source">Source file</param>
        /// <param name="destination">Destination</param>
        public static void cp(string source, string destination) {
            Console.WriteLine("Copying '{0}' to '{1}'", source, destination);
            File.Copy(source, destination);
        }

        /// <summary>
        ///   Deletes a file or directory
        /// </summary>
        /// <param name="file">
        ///   File or directory to delete
        /// </param>
        public static void rm(FileSystemInfo file) {
            if (file != null && file.Exists) {
                rm(file.FullName);
            }
        }

        /// <summary>
        ///   Deletes a file or directory
        /// </summary>
        /// <param name="path">
        ///   File or directory to delete
        /// </param>
        public static void rm(string path) {
            if (Directory.Exists(path)) {
                Console.WriteLine("Deleting directory '{0}'", path);
                DeleteDirectory(path);
            }
            else if (File.Exists(path)) {
                Console.WriteLine("Deleting file: '{0}'", path);
                DeleteFile(path);
            }
        }

        /// <summary>
        ///   Deletes a file or directory.
        /// </summary>
        /// <param name="path"></param>
        [Obsolete("use 'rm' instead or 'rmdir'")]
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
            SetAttributesNormal(dirInfo);
            dirInfo.Delete(true);
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
        ///   The Delete() method will fail with UnauthorizedAccessException if any files in the directory tree have the read-only flag. 
        ///   Delete() cannot delete anything with read-only flag even if the user running the application has priviliges to delete these files.
        /// </summary>
        /// <param name="dir"></param>
        static void SetAttributesNormal(DirectoryInfo dir) {
            // Remove flags from the current directory
            dir.Attributes = FileAttributes.Normal;

            // Remove flags from all files in the current directory
            foreach (FileInfo file in dir.GetFiles()) {
                file.Attributes = FileAttributes.Normal;
            }

            // Do the same for all subdirectories
            foreach (DirectoryInfo subDir in dir.GetDirectories()) {
                SetAttributesNormal(subDir);
            }
        }

        /// <summary>
        ///   Creates a directory if it does not exist.
        /// </summary>
        public static void mkdir(string path) {
            Console.WriteLine("Creating directory '{0}'", path);
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
        }
    }
}