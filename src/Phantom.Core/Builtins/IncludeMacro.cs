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

namespace Phantom.Core.Builtins {
    using System.IO;
    using Boo.Lang.Compiler;
    using Boo.Lang.Compiler.Ast;
    using IncludeSupport;

    public class IncludeMacro : AbstractAstMacro {
        public override Statement Expand(MacroStatement macro) {
            if (macro.Arguments.Count != 1)
                throw new ScriptParsingException("include requires a single literal string argument ('filename').");

            var include = macro.Arguments[0] as StringLiteralExpression;
            if (include == null)
                throw new ScriptParsingException("include argument should be a literal string ('filename').");

            var fullPath = Path.Combine(
                Path.GetDirectoryName(macro.LexicalInfo.FullPath),
                include.Value
            );

            var compiler = this.Context.GetService<IIncludeCompiler>();
            var compiled = compiler.CompileInclude(fullPath).CompileUnit.Modules[0];

            return compiled.Globals;
        }
    }
}
