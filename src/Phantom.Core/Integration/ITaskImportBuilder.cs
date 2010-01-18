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

namespace Phantom.Core.Integration {
	using System.Collections.Generic;
	using Boo.Lang.Compiler.Ast;

	public interface ITaskImportBuilder {
		IEnumerable<Import> BuildImportsFrom(string assemblyNameOrPath);
	}
}