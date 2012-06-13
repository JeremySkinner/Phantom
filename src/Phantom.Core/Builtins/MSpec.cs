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
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Machine.Specifications integration
    /// </summary>
    public class mspec : TestRunner<mspec> {
        public mspec() {
            toolPath = GetMSpecExePath();
            executable = "mspec-clr4.exe";
        }

        public string include { get; set; }
        public string exclude { get; set; }
        public string executable { get; set; }
        public bool enableTeamCity { get; set; }
        public string htmlOutput { get; set; }
        public string xmlOutput { get; set; }

        protected override void Execute() {
            if (!toolPath.EndsWith(".exe")) {
                toolPath = string.Format("{0}/{1}", toolPath, executable);
            }

            var args = new List<string>();

            if (enableTeamCity) {
                args.Add("--teamcity");
            }

            if (!string.IsNullOrEmpty(include)) {
                args.Add(string.Format("-i \"{0}\"", include));
            } else if (!string.IsNullOrEmpty(exclude)) {
                args.Add(string.Format("-x \"{0}\"", exclude));
            }

            if (!string.IsNullOrEmpty(htmlOutput)) {
                args.Add(string.Format("--html \"{0}\"", htmlOutput));
            }

            if (!string.IsNullOrEmpty(xmlOutput)) {
                args.Add(string.Format("--xml \"{0}\"", xmlOutput));
            }

            foreach (var asm in assemblies) {
                var mspecArgs = new List<string>(args) { string.Concat("\"", asm, "\"") };

                Execute(mspecArgs.JoinWith(" "));
            }
        }

        private static string GetMSpecExePath() {
            return Directory.Exists("packages/Machine.Specifications.0.5.6.0/tools") ? "packages/Machine.Specifications.0.5.6.0/tools/" : "lib/mspec";
        }
    }
}