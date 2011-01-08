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
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using System.Text;
	using Language;

	/// <summary>
	/// Generates AssemblyInfo files.
	/// Based on the AssemblyInfo rake task from Albacore http://github.com/derickbailey/Albacore/blob/master/lib/albacore/assemblyinfo.rb
	/// </summary>
	public class generate_assembly_info : IRunnable<generate_assembly_info> {
		const string RuntimeServicesNamespace = "System.Runtime.InteropServices";
		const string ReflectionNamespace = "System.Reflection";

		public IList namespaces { get; set; }

		public string file { get; set; }
		public string version { get; set; }
		public string title { get; set; }
		public string description { get; set; }
		public IDictionary customAttributes { get; set; }
		public string copyright { get; set; }
		public bool comVisible { get; set; }
		public string companyName { get; set; }
		public string productName { get; set; }
		public string fileVersion { get; set; }

		StringBuilder builder;

		public generate_assembly_info() {
			namespaces = new System.Collections.Generic.List<object>();
			customAttributes = new Dictionary<string, object>();
		}

		protected void Execute() {
			if (string.IsNullOrEmpty(file)) {
				throw new InvalidOperationException("'file' must be specified when calling generate_assembly_info");
			}

			EnsureDefaultNamespaces();

			var fileContents = BuildFileContents();

			File.WriteAllText(file, fileContents);
		}


		string BuildFileContents() {
			builder = new StringBuilder();

			foreach (var ns in namespaces) {
				builder.AppendLine(string.Format("using {0};", ns));
			}

			AddAttribute("AssemblyTitle", title);
			AddAttribute("AssemblyDescription", description);
			AddAttribute("AssemblyCompany", companyName);
			AddAttribute("AssemblyProduct", productName);
			AddAttribute("AssemblyCopyright", copyright);
			AddAttribute("ComVisible", comVisible);
			AddAttribute("AssemblyVersion", version);
			AddAttribute("AssemblyFileVersion", fileVersion);

			foreach (DictionaryEntry pair in customAttributes) {
				if (pair.Value is bool) {
					AddAttribute(pair.Key.ToString(), (bool) pair.Value);
				}
				else {
					AddAttribute(pair.Key.ToString(), pair.Value.ToString());
				}
			}

			return builder.ToString();
		}

		void AddAttribute(string attributeName, string value) {
			if (!string.IsNullOrEmpty(value)) {
				builder.AppendLine(string.Format("[assembly: {0}(\"{1}\")]", attributeName, value));
			}
		}


		void AddAttribute(string attributeName, bool? value) {
			if (value != null) {
				builder.AppendLine(string.Format("[assembly: {0}({1})]", attributeName, value.ToString().ToLowerInvariant()));
			}
		}

		void EnsureDefaultNamespaces() {
			if (! namespaces.Contains(ReflectionNamespace)) {
				namespaces.Add(ReflectionNamespace);
			}

			if (!namespaces.Contains(RuntimeServicesNamespace))
				namespaces.Add(RuntimeServicesNamespace);
		}

		public generate_assembly_info Run() {
			Execute();
			return this;
		}
	}
}