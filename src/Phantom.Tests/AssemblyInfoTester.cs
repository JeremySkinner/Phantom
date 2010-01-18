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

namespace Phantom.Tests {
	using System;
	using System.IO;
	using NUnit.Framework;

	[TestFixture]
	public class AssemblyInfoTester : ScriptTest {
		[Test]
		public void Generates_assembly_info_file() {
			ScriptFile = "Scripts/AssemblyInfoGenerator.boo";
			Execute("generate");

			string text = File.ReadAllText("TestAssemblyInfo.cs");
			string[] bits = text.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);

			bits[0].ShouldEqual("using Foo.Bar;");
			bits[1].ShouldEqual("using System.Reflection;");
			bits[2].ShouldEqual("using System.Runtime.InteropServices;");
			bits[3].ShouldEqual("[assembly: AssemblyTitle(\"Test1\")]");
			bits[4].ShouldEqual("[assembly: AssemblyDescription(\"Test2\")]");
			bits[5].ShouldEqual("[assembly: AssemblyCompany(\"Test4\")]");
			bits[6].ShouldEqual("[assembly: AssemblyProduct(\"Test5\")]");
			bits[7].ShouldEqual("[assembly: AssemblyCopyright(\"Test3\")]");
			bits[8].ShouldEqual("[assembly: ComVisible(true)]");
			bits[9].ShouldEqual("[assembly: AssemblyVersion(\"1.0.0.0\")]");
			bits[10].ShouldEqual("[assembly: AssemblyFileVersion(\"1.0.0.0\")]");
			bits[11].ShouldEqual("[assembly: Foo(\"Bar\")]");
		}
	}
}