namespace Phantom.Tests
{
	using System;
	using NUnit.Framework;
	using System.IO;

	[TestFixture]
	public class NugetPublishTester : ScriptTest
	{
		public override void Setup()
		{
			ScriptFile = @"Scripts/NugetPublish.boo";
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void Should_throw_exception_when_id_is_empty()
		{
			Execute("publishNugetPackageWithoutId");
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void Should_throw_exception_when_version_is_empty()
		{
			Execute("publishNugetPackageWithoutVersion");
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void Should_throw_exception_when_apikey_is_empty()
		{
			Execute("publishNugetPackageWithoutApiKey");
		}
	}
}