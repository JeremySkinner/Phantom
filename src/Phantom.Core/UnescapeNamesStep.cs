namespace Phantom.Core {
    using Boo.Lang.Compiler.Ast;
    using Boo.Lang.Compiler.Steps;

    public class UnescapeNamesStep : AbstractTransformerCompilerStep {
        public override void Run() {
            this.Visit(this.CompileUnit);
        }

        public override void OnReferenceExpression(ReferenceExpression node) {
            node.Name = node.Name.TrimStart('@');
            base.OnReferenceExpression(node);
        }

        public override void OnParameterDeclaration(ParameterDeclaration node) {
            node.Name = node.Name.TrimStart('@');
            base.OnParameterDeclaration(node);
        }

        public override void OnClassDefinition(ClassDefinition node) {
            node.Name = node.Name.TrimStart('@');
            base.OnClassDefinition(node);
        }

        public override void OnMethod(Method node) {
            node.Name = node.Name.TrimStart('@');
            base.OnMethod(node);
        }

        public override void OnProperty(Property node) {
            node.Name = node.Name.TrimStart('@');
            base.OnProperty(node);
        }
    }
}
