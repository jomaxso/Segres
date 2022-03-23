using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AnalyzerTools.Receiver
{
    public class AttributeSyntaxReceiver : SyntaxReceiver<ClassDeclarationSyntax>
    {
        protected override void OnValidSyntaxNodeVisit(ClassDeclarationSyntax syntax)
        {
            if (syntax.AttributeLists.Count > 0)
                this.Candidates.Add(syntax);
        }
    }
}