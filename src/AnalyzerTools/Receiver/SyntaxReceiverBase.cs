using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace AnalyzerTools.Receiver
{
    public abstract class SyntaxReceiverBase<TDeclaration, TInfo> : ISyntaxReceiver
        where TDeclaration : BaseTypeDeclarationSyntax
    {
        public List<TInfo> Candidates { get; set; } = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is TDeclaration syntax)
                this.OnValidSyntaxNodeVisit(syntax);
        }

        protected abstract void OnValidSyntaxNodeVisit(TDeclaration syntax);
    }
}