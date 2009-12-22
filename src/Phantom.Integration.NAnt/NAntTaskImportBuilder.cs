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
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using Boo.Lang.Compiler;
    using Boo.Lang.Compiler.Ast;
    using Boo.Lang.Compiler.IO;
    using Boo.Lang.Compiler.Pipelines;
    using Boo.Lang.Compiler.Steps;
    using Core.Language;
    using global::NAnt.Core;
    using global::NAnt.Core.Attributes;

    using Core;
    using Core.Integration;

    [Export(typeof(ITaskImportBuilder))]
    public class NAntTaskImportBuilder : ITaskImportBuilder {
        private const string Indent = "    ";
        private const string Indent2 = Indent + Indent;
        private const string Indent3 = Indent2 + Indent;

        #region TaskParameter Class

        private class TaskParameter {
            public string Name           { get; set; }
            public PropertyInfo Property { get; set; }
        }

        #endregion

        public Import BuildImportFrom(string assemblyNameOrPath) {
            var assembly = Assembly.Load(assemblyNameOrPath);
            var location = assembly.Location;
            var wrapperPath = Path.ChangeExtension(location, ".Phantom.dll");
            var @namespace = "IntegratedTasks";

            if (!File.Exists(wrapperPath))
                this.BuildImportWrapper(assembly, @namespace, wrapperPath);

            return new Import(@namespace, new ReferenceExpression(wrapperPath), null);
        }

        private void BuildImportWrapper(Assembly assembly, string @namespace, string wrapperPath) {
            // There are several solutions to this:
            // 1. Directly generate IL, optionally using some intermediate library
            // 2. Generate Boo AST and compile it
            // 3. Generate Boo code and compile it
            // I am going with option 3, just because it is the easiest to debug and understand later

            var tasks = (
                from type in assembly.GetExportedTypes()
                where type.IsSubclassOf(typeof(Task))
                   && !type.IsAbstract
                select type
            ).ToArray();
            var references = new HashSet<Assembly> { assembly, this.GetType().Assembly };

            var builder = new StringBuilder();
            builder.Append("namespace ").AppendLine(@namespace).AppendLine();

            builder.AppendLine("import Phantom.Core.Integration");
            builder.AppendLine("import Phantom.Core.Language");
            builder.AppendLine("import Phantom.Integration.NAnt");

            foreach (var task in tasks) {
                AppendTaskWrapper(builder, task, references);
                builder.AppendLine();
            }

            var code = builder.ToString();
            var compiler = new BooCompiler {
                Parameters = {
                    OutputType = CompilerOutputType.Library,
                    Pipeline = new CompileToFile(),
                    OutputAssembly = wrapperPath,
                    Input = { new StringInput("integration.boo", code) }
                }
            };
            compiler.Parameters.Pipeline.InsertBefore(typeof(ExpandAstLiterals), new UnescapeNamesStep());
            compiler.Parameters.Pipeline.InsertBefore(typeof(ProcessMethodBodiesWithDuckTyping), new AutoRunAllRunnablesStep());
            foreach (var reference in references) {
                compiler.Parameters.References.Add(reference);
            }

            var result = compiler.Run();
            if (result.Errors.Count > 0) {
                File.WriteAllText(Path.ChangeExtension(wrapperPath, ".boo"), code);
                throw new CompilerError(result.Errors.ToString(true));
            }
        }

        private void AppendTaskWrapper(StringBuilder builder, Type taskType, HashSet<Assembly> references) {
            var name = GetTaskName(taskType);

            builder.AppendFormat("class @{0}(IRunnable[of @{0}]):", name).AppendLine();
            AppendTaskWrapperConstructorAndField(builder, taskType);
            foreach (var parameter in GetTaskParameters(taskType)) {
                AppendTaskWrapperProperty(builder, parameter, references);
            }

            builder.Append(Indent).AppendLine("def Run():")
                   .Append(Indent2).AppendFormat("{0}().Run(task)", typeof(NAntTaskRunner).Name).AppendLine()
                   .Append(Indent2).AppendLine("return self");
        }

        private void AppendTaskWrapperConstructorAndField(StringBuilder builder, Type taskType) {
            builder.Append(Indent).Append("task as ").Append(taskType.FullName).AppendLine().AppendLine()
                   .Append(Indent).AppendLine("def constructor():")
                   .Append(Indent2).Append("task = ").Append(taskType.FullName).Append("()")
                   .AppendLine().AppendLine();
        }

        private void AppendTaskWrapperProperty(StringBuilder builder, TaskParameter parameter, HashSet<Assembly> references) {
            var type = parameter.Property.PropertyType;

            references.Add(type.Assembly);

            builder.Append(Indent).AppendFormat("@{0} as {1}:", parameter.Name, type.FullName.Replace('+', '.')).AppendLine();
            if (parameter.Property.GetGetMethod() != null) { // yes, there are such properties in NAnt
                builder.Append(Indent2).AppendLine("get:");
                builder.Append(Indent3).AppendLine("return task." + parameter.Property.Name);
            }
            if (parameter.Property.GetSetMethod() != null) {
                builder.Append(Indent2).AppendLine("set:");
                builder.Append(Indent3).AppendFormat("task.{0} = value", parameter.Property.Name).AppendLine().AppendLine();
            }
        }

        private string GetTaskName(Type taskType) {
            var nameAttribute = (TaskNameAttribute)System.Attribute.GetCustomAttribute(taskType, typeof(TaskNameAttribute));
            return nameAttribute != null ? nameAttribute.Name : taskType.Name.ToLowerInvariant();
        }

        private IEnumerable<TaskParameter> GetTaskParameters(Type taskType) {
            return from property in taskType.GetProperties()
                   let attribute = (TaskAttributeAttribute)System.Attribute.GetCustomAttribute(property, typeof(TaskAttributeAttribute))
                   where attribute != null
                   let name = attribute.Name.Replace("-", "")
                   select new TaskParameter { Name = name, Property = property };
        }
    }
}
