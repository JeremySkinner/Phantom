using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phantom.Tests
{
	using NUnit.Framework;
	using System.IO;

	[TestFixture]
	public class NugetPackTester : ScriptTest
	{
		public override void Setup() {
			ScriptFile = @"Scripts/NugetPack.boo";
		}

		[TearDown]
		protected void TearDown() {
			var directoryInfo = new DirectoryInfo(@"nuget/nuget_output");
			if (directoryInfo.Exists) {
				directoryInfo.Delete(true);
			}

			var packageInfo = new FileInfo("Phantom.1.0.nupkg");
			if(packageInfo.Exists) {
				packageInfo.Delete();
			}

			var tempPackage = new FileInfo(@"C:\Windows\Temp\Phantom.1.0.nupkg");
			if(tempPackage.Exists) {
				tempPackage.Delete();
			}
		}

		[Test]
		public void Should_create_nuget_package()
		{
			Execute("packNugetPackage");

			var packageInfo = new FileInfo("Phantom.1.0.nupkg");
			packageInfo.Exists.ShouldBeTrue();
		}

		[Test]
		public void Should_create_nuget_package_with_version_2_0_0() {
			Execute("packNugetPackageWithNewVersion");

			var packageInfo = new FileInfo(@"nuget/nuget_output/Phantom.2.0.0.nupkg");
			packageInfo.Exists.ShouldBeTrue();
		}

		[Test]
		public void Should_create_nuget_package_at_outputdirectory()
		{
			Execute("packNugetPackageWithOutputDirectory");

			var packageInfo = new FileInfo(@"nuget/nuget_output/Phantom.1.0.nupkg");
			packageInfo.Exists.ShouldBeTrue();
		}
	}
}
