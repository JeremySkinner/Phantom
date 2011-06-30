using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phantom.Tests
{
	using NUnit.Framework;
	using System.IO;

	[TestFixture]
	public class NugetTester : ScriptTest
	{
		public override void Setup() {
			ScriptFile = "Scripts/Nuget.boo";
		}

		[Test]
		public void Should_create_nuget_package_at_outputdirectory() {
			Execute("packNugetPackage");

			var packageInfo = new FileInfo("nuget_output/phantom.nupkg");
			packageInfo.Exists.ShouldBeTrue();
		}
	}
}
