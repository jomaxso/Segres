using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MicrolisR.Data.SourceGenerator
{
    public class MappableClass
    {
        private readonly string _name;

        public MappableClass(string name)
        {
            this._name = name;
        }

        public string GetName() => _name;

        public string GetNamespace(GeneratorExecutionContext context)
        {
            if (context.SemanticModel.GetDeclaredSymbol(ClassDeclarationSyntax) is INamedTypeSymbol symbol is false)
                throw new Exception();
            
            return symbol.ContainingNamespace.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        }
        
        public string GetFullName(GeneratorExecutionContext context) => string.Join(".", GetNamespace(context), GetName());
        public string GetMapperName(GeneratorExecutionContext context) => $"{GetFullName(context).Replace('.', '_')}MapperExtensions";

        public ClassDeclarationSyntax ClassDeclarationSyntax { get; set; }
    }
}