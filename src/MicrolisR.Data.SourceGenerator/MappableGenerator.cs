using AnalyzerTools.Models;
using AnalyzerTools.Receiver;
using MicrolisR.Data.SourceGenerator.MappableInfo;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicrolisR.Data.SourceGenerator
{
    [Generator]
    public class MappableGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new AttributeSyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var infos = context.GetMappableClassInfos().ToList();

            var privateMappingMethodSource = infos.GeneratePrivateMappingMethods();
            var listOfAttributeTypes = GetListOfTypesByAttributeTypes(infos);

            StringBuilder sourceBuilder = new();

            sourceBuilder.Append($@"
namespace MicrolisR.Data.Mapping
{{")
                .Append($@"
    internal static class Mapper
    {{");

            foreach (var info in infos)
            {
                var index = listOfAttributeTypes.FindIndex(
                    obj => obj.Attribute.FullName?.Equals(info.FullName) ?? false);

                if (index < 0)
                    continue;

                bool infoFound = false;

                foreach (var attribute in info.Attributes)
                {
                    if (attribute is not MappableAttributeInfo mappableAttributeInfo)
                        continue;

                    var listOfProperties = new List<PropInfo>();

                    foreach (var propertyName in mappableAttributeInfo.PropertyNames)
                    {
                        listOfProperties.Add(new PropInfo(propertyName, ""));
                    }

                    listOfAttributeTypes[index].ClassList.Add(new ClassInfo(mappableAttributeInfo.Namespace!,
                        "internal", "class", mappableAttributeInfo.ClassName, listOfProperties));

                    infoFound = true;
                }

                if (infoFound)
                    continue;

                sourceBuilder.Append($@"
        internal static T MapTo<T>(this {info.FullName} obj) 
            where T : class, new() 
            => typeof(T).FullName switch
        {{");
                foreach (var attribute in info.Attributes)
                {
                    if (attribute is not MappableAttributeInfo mappableAttributeInfo)
                        continue;

                    string compareNameString = $"\"{mappableAttributeInfo.FullName}\"";

                    sourceBuilder.Append(@$"
            {compareNameString} => obj.To{mappableAttributeInfo.ClassName}() as T,");
                }

                var exceptionText =
                    $"$\"You may forgot to use the MappableAttribute within the class {{typeof(T).FullName}} or {info.FullName}\"";

                sourceBuilder.Append($@"
            
            _ => throw new global::System.ArgumentException({exceptionText})
        }};
");
            }

            foreach (var (mappableAttributeInfo, classInfos) in listOfAttributeTypes)
            {
                sourceBuilder.Append($@"
        internal static T MapTo<T>(this {mappableAttributeInfo.FullName} obj) 
            where T : class, new() 
            => typeof(T).FullName switch
        {{");
                foreach (var classInfo in classInfos)
                {
                    string compareNameString = $"\"{classInfo.FullName}\"";

                    sourceBuilder.Append(@$"
            {compareNameString} => obj.To{classInfo.Name}() as T,");
                }

                var exceptionText =
                    $"$\"You may forgot to use the MappableAttribute within the class {{typeof(T).FullName}} or {mappableAttributeInfo.FullName}\"";

                sourceBuilder.Append($@"
            
            _ => throw new global::System.ArgumentException({exceptionText})
        }};
");
            }


            sourceBuilder.Append(privateMappingMethodSource);
            sourceBuilder.Append($@"
    }}
}}");


            context.AddSource($"MicrolisR.Data.Mapping.Mapper.g.cs",
                SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        }


        private static List<(MappableAttributeInfo Attribute, List<ClassInfo> ClassList)>
            GetListOfTypesByAttributeTypes(IEnumerable<ClassInfo> infos)
        {
            List<(MappableAttributeInfo Attribute, List<ClassInfo> ClassList)> reverseList = new();

            foreach (var info in infos)
            {
                foreach (var attribute in info.Attributes)
                {
                    if (attribute is not MappableAttributeInfo mappableAttributeInfo)
                        continue;

                    if (reverseList.Any(o => o.Attribute.FullName?.Equals(mappableAttributeInfo.FullName) ?? false))
                    {
                        var index = reverseList
                            .FindIndex(o => o.Attribute.FullName?
                                .Equals(mappableAttributeInfo.FullName) ?? false);

                        if (index < 0)
                            continue;

                        reverseList[index].ClassList.Add(info);

                        continue;
                    }

                    reverseList.Add(new(mappableAttributeInfo, new() { info }));
                }
            }

            return reverseList;
        }
    }



}
