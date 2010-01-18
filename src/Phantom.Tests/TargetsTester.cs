#region License (+ashmind)

// Copyright Jeremy Skinner (http://www.jeremyskinner.co.uk)
// Copyright Andrey Shchekin (http://ashmind.com)
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
    using NUnit.Framework;

    [TestFixture]
    public class TargetsTester : ScriptTest {
        public override void Setup() {
            ScriptFile = "Scripts/TargetNames.boo";
        }

        [Test]
        public void Executes_target_with_complex_name() {
            Execute("db.migration.retry");
            AssertOutput("db.migration.retry:", "in db.migration.retry");
        }
    }
}
