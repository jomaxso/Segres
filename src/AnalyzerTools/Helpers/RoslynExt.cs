using AnalyzerTools.Models;
using AnalyzerTools.Receiver;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerTools.Helpers
{

    public static class RoslynExt
    {
        internal static Compilation AddToCompilation(this GeneratorExecutionContext context, string source)
        {
            return AddToCompilation(context.Compilation, source);
        }

        internal static Compilation AddToCompilation(this Compilation compilation, string source)
        {
            var options = (compilation as CSharpCompilation)?.SyntaxTrees[0].Options as CSharpParseOptions;

            SyntaxTree syntaxTree =
                CSharpSyntaxTree.ParseText(SourceText.From(source, Encoding.UTF8),
                    options);

            return compilation.AddSyntaxTrees(syntaxTree);
        }


        internal static IEnumerable<ClassInfo> GetInfosByAttribute(this GeneratorExecutionContext context,
            string typeMetaName)
        {
            // if (context.SyntaxReceiver.TryParse(out SyntaxReceiver<T>? receiver)) // todo fehler !!! 
            //     return Array.Empty<ClassInfo>();

            if (context.SyntaxReceiver is not AttributeSyntaxReceiver receiver)
                return Array.Empty<ClassInfo>();

            var attributeSymbol =
                context.Compilation.GetTypeByMetadataName(typeMetaName);


            if (attributeSymbol is null)
                return Array.Empty<ClassInfo>();

            List<ClassInfo> outputList = new();

            foreach (var syntax in receiver.Candidates)
            {

                SemanticModel classModel = context.Compilation.GetSemanticModel(syntax.SyntaxTree);
                ISymbol? classSymbol = ModelExtensions.GetDeclaredSymbol(classModel, syntax);

                if (classSymbol is null)
                    continue;

                // Do not generate an overload if we do not find our attribute on the class in question todo error
                if (!(classSymbol.GetAttributes().Any(a =>
                        a.AttributeClass!.Equals(attributeSymbol, SymbolEqualityComparer.Default))))
                    continue;

                var classInfo = context.GetClassInfo(syntax);

                if (classInfo is null || outputList.Any(c => c.FullName.Equals(classInfo.FullName)))
                    continue;

                // todo AddPropertyInfos(classDeclaration, classModel, classInfo);

                outputList.Add(classInfo);
            }

            return outputList;
        }
    }
}