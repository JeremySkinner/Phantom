namespace Phantom.Tests
{
	using System;
	using NUnit.Framework;
	using System.IO;

	[TestFixture]
	public class NugetPushTester : ScriptTest
	{
		public override void Setup()
		{
			ScriptFile = @"Scripts/NugetPush.boo";
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void Should_throw_exception_when_packagepath_is_empty()
		{
			Execute("pushNugetPackageWithoutPackage");
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void Should_throw_exception_when_apikey_is_empty()
		{
			Execute("pushNugetPackageWithoutApiKey");
		}
	}
}