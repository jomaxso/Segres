using System.Reflection.Metadata;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

using System.Text;

namespace MicrolisR.Data.SourceGenerator
{
    [Generator]
    public class MappableGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            if ((context.SyntaxReceiver is MappableSyntaxReceiver syntaxReceiver) is false) 
                return;

            foreach (var @class in syntaxReceiver.MappableClasses)
            {
                var source = new StringBuilder();
                
                source.AppendLine("namespace MicrolisR.Data.Mapping");
                source.AppendLine("{");
                source.AppendLine($"     internal static partial class {@class.MapperName}");
                source.AppendLine("     {");
                source.AppendLine($"         public static {@class.GetFullName(context)} To{@class.GetName()}(this Demo.Class2 class2)");
                source.AppendLine("         {");
                source.AppendLine($"             return new {@class.FullName}()");
                source.AppendLine("             {");
                source.AppendLine("                 MyProperty = class2.MyProperty,");
                source.AppendLine("             };");
                source.AppendLine("         }");
                source.AppendLine("     }");
                source.AppendLine("}");

                context.AddSource($"MicrolisR.Data.Mapping.{@class.MapperName}.Generated.cs",
                    SourceText.From(source.ToString(), Encoding.UTF8));
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
#if DEBUG
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                // System.Diagnostics.Debugger.Launch();
            }
#endif

            context.RegisterForSyntaxNotifications(() => new MappableSyntaxReceiver());
        }
    }
}