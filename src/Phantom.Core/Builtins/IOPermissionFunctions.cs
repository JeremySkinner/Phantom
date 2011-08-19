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
	using System.IO;
	using System.Security.AccessControl;
	using System.Runtime.CompilerServices;

	[CompilerGlobalScope]
	public static class IOPermissionFunctions {
		public static void SetFilePermission(string path, string accountName, PermissionLevel permissionLevel) {
			AddFileSecurity(path, accountName, GetRightsFromPermissionLevel(permissionLevel));
		}

		public static void SetDirectoryPermission(string path, string accountName, PermissionLevel permissionLevel) {
			AddDirectorySecurity(path, accountName, GetRightsFromPermissionLevel(permissionLevel));
		}

		static void AddFileSecurity(string path, string account, FileSystemRights rights) {
			FileInfo fileInfo = new FileInfo(path.Replace('\\', '/'));
			FileSecurity fileSecurity = fileInfo.GetAccessControl();
			fileSecurity.AddAccessRule(new FileSystemAccessRule(account,
			                                                    rights,
			                                                    AccessControlType.Allow
			                           	));

			fileInfo.SetAccessControl(fileSecurity);
		}

		static void AddDirectorySecurity(string path, string account, FileSystemRights rights) {
			DirectoryInfo direcotryInfo = new DirectoryInfo(path.Replace('\\', '/'));
			DirectorySecurity directorySecurity = direcotryInfo.GetAccessControl();
			directorySecurity.AddAccessRule(new FileSystemAccessRule(account,
			                                                         rights, InheritanceFlags.ContainerInherit, PropagationFlags.InheritOnly,
			                                                         AccessControlType.Allow));
			directorySecurity.AddAccessRule(new FileSystemAccessRule(account,
			                                                         rights, InheritanceFlags.ObjectInherit, PropagationFlags.InheritOnly,
			                                                         AccessControlType.Allow));
			directorySecurity.AddAccessRule(new FileSystemAccessRule(account,
			                                                         rights, AccessControlType.Allow));
			direcotryInfo.SetAccessControl(directorySecurity);

		}

		static FileSystemRights GetRightsFromPermissionLevel(PermissionLevel level) {
			if (level == PermissionLevel.Full)
				return FileSystemRights.FullControl;

			if (level == PermissionLevel.Modify)
				return FileSystemRights.Modify;

			if (level == PermissionLevel.Write)
				return FileSystemRights.Write;

			if (level == PermissionLevel.ReadAndExecute)
				return FileSystemRights.ReadAndExecute;

			return FileSystemRights.Read;
		}

	}

	public enum PermissionLevel {
		Read,
		ReadAndExecute,
		Write,
		Modify,
		Full
	}
}