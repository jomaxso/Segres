using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace AnalyzerTools.Receiver
{
    public static class SyntaxReceiver
    {
        public static bool TryParse<T>(this ISyntaxReceiver? syntaxReceiver, out T? receiver)
            where T : class, ISyntaxReceiver => syntaxReceiver switch
            {
                AttributeSyntaxReceiver sr => (receiver = sr as T) is not null,
                EnumSyntaxReceiver sr => (receiver = sr as T) is not null,
                _ => (receiver = syntaxReceiver as T) is not null
            };
    }

    public abstract class SyntaxReceiver<T> : ISyntaxReceiver
        where T : BaseTypeDeclarationSyntax
    {
        public List<T> Candidates { get; } = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is T syntax)
                this.OnValidSyntaxNodeVisit(syntax);
        }


        protected virtual void OnValidSyntaxNodeVisit(T syntax)
        {
            this.Candidates.Add(syntax);
        }
    }
}