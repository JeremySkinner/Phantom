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

namespace Phantom.Core {
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;

	public class ScriptModel : IEnumerable<Target> {
		public const string DefaultTargetName = "default";
		
		Func<TextWriter> log = () => Console.Out;
		
		public TextWriter Log {
			get { return log(); }
			set { log = () => value; }
		}

		readonly Dictionary<string, Target> targets = new Dictionary<string, Target>();
		readonly List<Cleanup> cleanups = new List<Cleanup>();

		public Target DefaultTarget {
			get { return GetTarget(DefaultTargetName); }
		}

		public Target GetTarget(string name) {
			return targets.ValueOrDefault(name);
		}

		public IList<Cleanup> GetCleanups() {
			var localCleanups = new Cleanup[cleanups.Count];
			cleanups.CopyTo(localCleanups);
			return localCleanups;
		}

		public IEnumerator<Target> GetEnumerator()
		{
			return targets.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		public void AddTarget(string name, string[] dependencies, Action block, string description) {
			var target = new Target(name, block, dependencies, description, this);
			if (targets.ContainsKey(target.Name)) {
				throw new TargetAlreadyExistsException(target.Name);
			}
			targets.Add(target.Name, target);
		}

		public void AddCleanup(string name, Action block, string description)
		{
			var cleanup = new Cleanup(name, block, description, this);
			cleanups.Add(cleanup);
		}
		
		public void ExecuteTargets(params string[] targetNames)
		{
			var targetsToExecute = new List<Target>();

			foreach (var targetName in targetNames) {
				var rootTarget = GetTarget(targetName);

				if (rootTarget == null) {
					throw new TargetNotFoundException(targetName);
				}

				foreach (var targetInSequence in rootTarget.GetExecutionSequence()) {
					if (! targetsToExecute.Contains(targetInSequence)) {
						targetsToExecute.Add(targetInSequence);
					}
				}
			}

			try {
				foreach (var target in targetsToExecute) {
					Log.WriteLine(target.Name + ":");
					target.Execute();
					Log.WriteLine();
				}
			}
			catch (PhantomException e) {
				Log.WriteLine(string.Format("Target failed: {0}", e.Message));
				Environment.ExitCode = 1;
			}
		}

		public void ExecuteCleanups() {
			foreach (var cleanup in cleanups) {
				try {
					Log.Write("cleanup");
					if (cleanup.Name != null) {
						Log.Write(" " + cleanup.Name);
					}
					Log.WriteLine(":");
					cleanup.Execute();
					Log.WriteLine();
				}
				catch (Exception e) {
					Log.WriteLine(string.Format("Cleanup failed: {0}", e.Message));
				}
			}
		}
	}
}