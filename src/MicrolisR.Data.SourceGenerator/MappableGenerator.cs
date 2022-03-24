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
            StringBuilder source = new StringBuilder();

            source.AppendLine("namespace TEst;");
            source.AppendLine();
            source.AppendLine("public class Test{}");


            context.AddSource("Test", SourceText.From(source.ToString(), Encoding.UTF8));
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
