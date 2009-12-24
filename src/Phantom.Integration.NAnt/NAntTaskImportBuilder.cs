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

        public IEnumerable<Import> BuildImportsFrom(string assemblyNameOrPath) {
            var assembly = Assembly.Load(assemblyNameOrPath);
            var location = assembly.Location;

            var adapterPath = Path.ChangeExtension(location, ".Phantom.dll");
            var importsPath = Path.ChangeExtension(location, ".Phantom.imports");
            var adapterNamespace = "PhantomIntegratedTasks";

            if (!File.Exists(adapterPath))
                this.BuildAdapter(assembly, adapterNamespace, adapterPath, importsPath);

            var otherImports = File.ReadAllLines(importsPath).Select(line => line.Split(new[] { ',' }, 2));
            foreach (var otherImport in otherImports) {
                yield return new Import(otherImport[0], new ReferenceExpression(otherImport[1]), null);
            }

            yield return new Import(
                adapterNamespace, new ReferenceExpression(adapterPath), null
            );
        }

        private void BuildAdapter(Assembly assembly, string adapterNamespace, string adapterPath, string importsPath) {
            // There are several solutions to this:
            // 1. Directly generate IL, optionally using some intermediate library
            // 2. Generate Boo AST and compile it
            // 3. Generate Boo code and compile it
            // I am going with option 3, just because it is the easiest to debug and understand later

            var referencedTypes = new HashSet<Type>();
            var code = GenerateAdapterCode(assembly, adapterNamespace, referencedTypes);

            var compiler = new BooCompiler {
                Parameters = {
                    OutputType = CompilerOutputType.Library,
                    Pipeline = new CompileToFile(),
                    OutputAssembly = adapterPath,
                    Input = { new StringInput("integration.boo", code) }
                }
            };
            compiler.Parameters.Pipeline.InsertBefore(typeof(ExpandAstLiterals), new UnescapeNamesStep());
            compiler.Parameters.Pipeline.InsertBefore(typeof(ProcessMethodBodiesWithDuckTyping), new AutoRunAllRunnablesStep());

            compiler.Parameters.References.Add(assembly);
            compiler.Parameters.References.Add(this.GetType().Assembly);
            foreach (var referencedAssembly in referencedTypes.Select(t => t.Assembly).Distinct()) {
                compiler.Parameters.References.Add(referencedAssembly);
            }

            var result = compiler.Run();
            if (result.Errors.Count > 0) {
                File.WriteAllText(Path.ChangeExtension(adapterPath, ".boo"), code);
                throw new CompilerError(result.Errors.ToString(true));
            }

            using (var writer = new StreamWriter(importsPath, false)) {
                var imports = referencedTypes.Select(type => new {type.Namespace, type.Assembly}).Distinct();
                foreach (var import in imports) {
                    writer.WriteLine(import.Namespace + "," + import.Assembly.Location);
                }
            }
        }

        private string GenerateAdapterCode(Assembly assembly, string adapterNamespace, HashSet<Type> referencedTypes) {
            var tasks = (
                from type in assembly.GetExportedTypes()
                where type.IsSubclassOf(typeof(Task))
                   && !type.IsAbstract
                select type
            ).ToArray();

            var builder = new StringBuilder();
            builder.Append("namespace ").AppendLine(adapterNamespace).AppendLine();

            builder.AppendLine("import Phantom.Core.Integration");
            builder.AppendLine("import Phantom.Core.Language");
            builder.AppendLine("import Phantom.Integration.NAnt");

            foreach (var task in tasks) {
                AppendTaskAdapter(builder, task, referencedTypes);
                builder.AppendLine();
            }

            return builder.ToString();
        }

        private void AppendTaskAdapter(StringBuilder builder, Type taskType, HashSet<Type> referencedTypes) {
            var name = GetTaskName(taskType);

            builder.AppendFormat("class @{0}(IRunnable[of @{0}]):", name).AppendLine();
            AppendTaskAdapterConstructorAndField(builder, taskType);
            foreach (var parameter in GetTaskParameters(taskType)) {
                AppendTaskAdapterProperty(builder, parameter, referencedTypes);
            }

            builder.Append(Indent).AppendLine("def Run():")
                   .Append(Indent2).AppendFormat("{0}().Run(task)", typeof(NAntTaskRunner).Name).AppendLine()
                   .Append(Indent2).AppendLine("return self");
        }

        private void AppendTaskAdapterConstructorAndField(StringBuilder builder, Type taskType) {
            builder.Append(Indent).Append("task as ").Append(taskType.FullName).AppendLine().AppendLine()
                   .Append(Indent).AppendLine("def constructor():")
                   .Append(Indent2).Append("task = ").Append(taskType.FullName).Append("()")
                   .AppendLine().AppendLine();
        }

        private void AppendTaskAdapterProperty(StringBuilder builder, TaskParameter parameter, HashSet<Type> referencedTypes) {
            var type = parameter.Property.PropertyType;
            referencedTypes.Add(type);

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
