namespace Spectre.Core.Builtins {
	using System;
	using System.Diagnostics;
	using System.IO;
	using System.Runtime.CompilerServices;
	using System.Threading;

	[CompilerGlobalScope]
	public sealed class IOFunctions {
		/// <summary>
		/// Executes the specified program with the specified arguments
		/// </summary>
		/// <param name="command">The command to execute</param>
		/// <param name="args">Additional args</param>
		public static void exec(string command, string args) {
			var psi = new ProcessStartInfo(command, args) {
				UseShellExecute = false,
				RedirectStandardError = true
			};
			var process = Process.Start(psi);
			process.WaitForExit();
		}

		public static void exec(string command) {
			string commandPrompt = UtilityFunctions.env("COMSPEC");
			string args = string.Format("/C \"{0}\"", command);

			exec(commandPrompt, args);
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