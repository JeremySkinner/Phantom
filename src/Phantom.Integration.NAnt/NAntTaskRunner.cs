#region License (+ashmind)

// Copyright Jeremy Skinner (http://www.jeremyskinner.co.uk)
// Copyright Andrey Shchekin (http://www.ashmind.com)
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
// 
// The latest version of this file can be found at http://github.com/ashmind/Phantom

#endregion

namespace Phantom.Integration.NAnt {
    using System;
    using System.Reflection;
    using System.Xml;
    using global::NAnt.Core;

    public class NAntTaskRunner {
        public void Run(Task task) {
            task.Project = CreateProject();
            task.Execute();
        }

        private Project CreateProject() {
            var emptyProjectXml = new XmlDocument();
            emptyProjectXml.LoadXml("<project />");

            // yes I know it is a hack, but it works so perfectly as compared to public constructors
            // that require piles of configuration
            var project = (Project)Activator.CreateInstance(
                typeof(Project), BindingFlags.Instance | BindingFlags.NonPublic,
                null, new object[] {emptyProjectXml}, null
            );
            project.AttachBuildListeners(new BuildListenerCollection(new[] { new DefaultLogger() }));

            return project;
        } 
    }
}
