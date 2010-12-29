namespace Phantom.Core {
	using System;
	using System.Linq;
	using System.Reflection;

	// base class for CSharp build scripts.
	public abstract class BuildScript {
		public delegate void Target();

		internal ScriptModel GetScriptModel() {
			var query = from field in GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
						where field.FieldType == typeof(Target)
						select new {
						  name = field.Name,
         				  accessor = (Target)field.GetValue(this)
						};

			var model = new ScriptModel();
			foreach(var target in query) {
				var t = target;
				model.AddTarget(target.name, new string[0], () => t.accessor());
			}

			return model;
		}
	}
}