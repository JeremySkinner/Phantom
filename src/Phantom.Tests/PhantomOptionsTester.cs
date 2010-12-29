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
	using System.Linq;
	using Core;
	using NUnit.Framework;

	[TestFixture]
	public class PhantomOptionsTester {
		PhantomOptions args;

		[SetUp]
		public void Setup() {
			args = new PhantomOptions();
		}

		[Test]
		public void Sensible_Defaults() {
			args.Parse(new string[0]);
			args.File.ShouldEqual("build.boo");
			args.Help.ShouldEqual(false);
			args.TargetNames.Single().ShouldEqual("default");
		}

		[Test]
		public void Parses_help() {
			args.Parse(new[] {"-h"});
			args.Help.ShouldBeTrue();
		}

		[Test]
		public void Parses_file() {
			args.Parse(new[] {"-f:test.boo"});
			args.File.ShouldEqual("test.boo");
		}

		[Test]
		public void Parses_file_without_colon() {
			args.Parse(new[] { "-f test.boo"  });
			args.File.ShouldEqual("test.boo");
		}

		[Test]
		public void Custom_target_names() {
			args.Parse(new[] {"foo", "bar"});
			args.TargetNames.Count().ShouldEqual(2);
			args.TargetNames.First().ShouldEqual("foo");
			args.TargetNames.Last().ShouldEqual("bar");
		}

		[Test]
		public void Parses_targets() {
			args.Parse(new[] {"-t"});
			args.ShowTargets.ShouldBeTrue();
		}

		[Test]
		public void Adds_arguments_to_environment() {
			args.Parse(new[] {"-a:foo=bar", "-a:boo=baz"});
			Environment.GetEnvironmentVariable("foo").ShouldEqual("bar");
			Environment.GetEnvironmentVariable("boo").ShouldEqual("baz");
		}

		[Test]
		public void Lots_of_arguments() {
			args.Parse(new[] { "-f test.boo", "-t", "-h", "-a:foo=bar", "-a:boo=baz", "target1", "target2" });
			args.File.ShouldEqual("test.boo");
			args.ShowTargets.ShouldBeTrue();
			args.Help.ShouldBeTrue();
				Environment.GetEnvironmentVariable("foo").ShouldEqual("bar");
			Environment.GetEnvironmentVariable("boo").ShouldEqual("baz");
			args.TargetNames.Count().ShouldEqual(2);
		}
	}
}