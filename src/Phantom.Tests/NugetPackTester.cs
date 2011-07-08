namespace Phantom.Tests
{
	using System;
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
		}

		[Test]
		public void Should_create_nuget_package_at_currentdirectory() {
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
		public void Should_create_nuget_package_at_outputdirectory() {
			Execute("packNugetPackageWithOutputDirectory");

			var packageInfo = new FileInfo(@"nuget/nuget_output/Phantom.1.0.nupkg");
			packageInfo.Exists.ShouldBeTrue();
		}

		[Test]
		public void Should_create_nuget_package_with_symbols() {
			Execute("packNugetPackageWithSymbols");

			var packageInfo = new FileInfo(@"nuget/nuget_output/Phantom.1.0.nupkg");
			var symbolsInfo = new FileInfo(@"nuget/nuget_output/Phantom.1.0.symbols.nupkg");
			
			packageInfo.Exists.ShouldBeTrue();
			symbolsInfo.Exists.ShouldBeTrue();
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void Should_throw_exception_when_nuspecfile_is_empty() {
			Execute("packNugetPackageNoNuspec");
		}
	}
}
