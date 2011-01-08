namespace Phantom.DSL.CSharp {
	using System;
	using System.ComponentModel.Composition;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	using Core;
	using CSScriptLibrary;

	[Export(typeof(IDslFactory))]
	public class CSharpDslFactory : IDslFactory {
		public bool CanExecute(string path) {
			var ext = Path.GetExtension(path);
			return ext == ".cs";
		}

		public ScriptModel BuildModel(string path) {
			var code = GenerateCode(path);
			var assembly = CSScript.LoadCode(code);
			var type = assembly.GetType("Scripting.DynamicClass");
			var model = new ScriptModel();

			if(type == null) {
				throw new InvalidOperationException("Could not load the script. No generated classes were found.");
			}
			
			var scriptInstance = Activator.CreateInstance(type);

			var targets = from field in type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
						  let isTarget = field.FieldType.Name == "Target"
						  where isTarget
						  let target = (Delegate)field.GetValue(scriptInstance)
						  let targetAction = new Action(() => target.DynamicInvoke())
						  let description = GetDescription(field)
						  let dependencies = GetDependencies(field)
						  select new { action = targetAction, name = field.Name, description, dependencies };

			foreach(var target in targets) {
				model.AddTarget(target.name, target.dependencies, target.action, target.description);
			}

			return model;
		}


		private string GetDescription(FieldInfo field) {
			var attributes = field.GetCustomAttributes(false);
			var descAttr = attributes.Where(x => x.GetType().Name == "DescAttribute").FirstOrDefault();
			if (descAttr == null) return null;
			return descAttr.GetType().GetProperty("Name").GetValue(descAttr, null) as string;
		}

		private string[] GetDependencies(FieldInfo field) {
			var attributes = field.GetCustomAttributes(false);
			var dependsAttr = attributes.Where(x => x.GetType().Name == "DependsAttribute").FirstOrDefault();
			if(dependsAttr == null) return new string[0];
			return dependsAttr.GetType().GetProperty("Targets").GetValue(dependsAttr, null) as string[];
		}

		// This code is based on the LoadMethod code from CSScriptLib.
		private string GenerateCode(string path) {
			string scriptCode = File.ReadAllText(path);

			var code = new StringBuilder(4096);
			code.Append("using System;\r\n");

			bool headerProcessed = false;
			string line;
			using (var reader = new StringReader(scriptCode))
				while ((line = reader.ReadLine()) != null) {
					if (!headerProcessed && !line.TrimStart().StartsWith("using ")) //not using...; statement of the file header
						if (!line.StartsWith("//") && line.Trim() != "") //not comments or empty line
                            {
							headerProcessed = true;
							code.Append("namespace Scripting\r\n");
							code.Append("{\r\n");
							code.Append("   public class DynamicClass\r\n");
							code.Append("   {\r\n");
							code.Append("       protected delegate void Target();\r\n");
							code.Append("       [AttributeUsage(AttributeTargets.Field)]\r\n");
							code.Append("       protected class DescAttribute : Attribute {\r\n");
							code.Append("           public string Name { get; private set; }\r\n");
							code.Append("           public DescAttribute(string name) {\r\n");
							code.Append("             Name = name;\r\n");
							code.Append("           }\r\n");
							code.Append("       }\r\n");

							code.Append("       [AttributeUsage(AttributeTargets.Field)]\r\n");
							code.Append("       protected class DependsAttribute : Attribute {\r\n");
							code.Append("           public string[] Targets { get; private set; }\r\n");
							code.Append("           public DependsAttribute(params string[] targets) {\r\n");
							code.Append("             Targets = targets;\r\n");
							code.Append("           }\r\n");
							code.Append("       }\r\n");




						}

					code.Append(line);
					code.Append("\r\n");
				}

			code.Append("   }\r\n");
			code.Append("}\r\n");

			return code.ToString();
		}
	}
}