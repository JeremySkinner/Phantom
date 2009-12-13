#region License

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

namespace Phantom.Core.Builtins.IncludeSupport {
    using Boo.Lang.Compiler;

    public class IncludeSupportStep : ICompilerStep {
        readonly IIncludeCompiler includeCompiler;

        public IncludeSupportStep(IIncludeCompiler includeCompiler) {
            this.includeCompiler = includeCompiler;
        }

        public void Run() {
        }

        public void Initialize(CompilerContext context) {
            context.RegisterService(this.includeCompiler);
        }

        public void Dispose() {
        }
    }
}
