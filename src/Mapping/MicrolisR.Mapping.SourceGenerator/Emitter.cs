using System;
using System.Linq;
using System.Text;
using System.Threading;
using MicrolisR.Mapping.SourceGenerator.Models;

namespace MicrolisR.Mapping.SourceGenerator;

internal class Emitter
{
    public string Emit(MappableClass mappableClass, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return string.Empty;

        var source = new StringBuilder();
        AppendMappableClassSource(source, mappableClass);
        return source.ToString();
    }

    private static void AppendMappableClassSource(StringBuilder source, MappableClass mappableClass)
    {
        source.AppendLine("#nullable enable");
        source.AppendLine();
        source.AppendLine($"namespace {mappableClass.Namespace}");
        source.AppendLine("{");
        source.AppendLine("     internal partial class MicrolisRSourceGeneratorMappers");
        source.AppendLine("     {");

        foreach (var mapToClass in mappableClass.MapToClasses)
        {
            source.AppendLine($"        private class {mappableClass.Name}_{mapToClass.Name}_Mapper :");
            source.AppendLine(
                $"            MicrolisR.Mapping.IMapperDefinition<{mappableClass.FullName}, {mapToClass.FullName}>,");
            source.AppendLine(
                $"            MicrolisR.Mapping.IMapperDefinition<{mapToClass.FullName}, {mappableClass.FullName}>");
            source.AppendLine("         {");
            source.AppendLine();
            source.AppendLine("             private readonly MicrolisR.Mapping.IMapper _mapper;");
            source.AppendLine();
            source.AppendLine(
                $"             public {mappableClass.Name}_{mapToClass.Name}_Mapper(MicrolisR.Mapping.IMapper mapper)");
            source.AppendLine("             {");
            source.AppendLine("                 this._mapper = mapper;");
            source.AppendLine("             }");
            source.AppendLine();
            source.AppendLine($"            public {mappableClass.FullName} Map({mapToClass.FullName} value)");
            source.AppendLine("             {");
            source.AppendLine($"                return new {mappableClass.FullName}()");
            source.AppendLine("                 {");
            foreach (var property in mappableClass.Properties)
            {
                var propertyToMap = mapToClass.Properties.FirstOrDefault(p => p.MapName == property.MapName);

                if (propertyToMap is null)
                    continue;

                var text = GetConvertText(property, propertyToMap);

                if (string.IsNullOrWhiteSpace(text) is false)
                    source.AppendLine($"                    {text}");
            }

            source.AppendLine("                 };");
            source.AppendLine("             }");


            source.AppendLine();


            source.AppendLine($"            public {mapToClass.FullName} Map({mappableClass.FullName} value)");
            source.AppendLine("             {");
            source.AppendLine($"                return new {mapToClass.FullName}()");
            source.AppendLine("                 {");
            foreach (var property in mappableClass.Properties)
            {
                var propertyToMap = mapToClass.Properties.FirstOrDefault(p => p.MapName == property.MapName);

                if (propertyToMap is null)
                    continue;
                
                var text = GetConvertText(propertyToMap, property);

                if (string.IsNullOrWhiteSpace(text) is false)
                    source.AppendLine($"                    {text}");
            }

            source.AppendLine("                 };");
            source.AppendLine("             }");


            source.AppendLine();


            source.AppendLine("             public T? Handle<T>(object value)");
            source.AppendLine("             {");
            source.AppendLine("                 return value switch");
            source.AppendLine("                 {");
            source.AppendLine(
                $"                    {mappableClass.FullName} source => this.Map(source) is T sourceResult ? sourceResult : default,");
            source.AppendLine(
                $"                    {mapToClass.FullName} target => this.Map(target) is T targetResult ? targetResult : default,");
            source.AppendLine("                     _ => default");
            source.AppendLine("                 };");
            source.AppendLine("             }");


            source.AppendLine("         }");
            source.AppendLine();
        }

        source.AppendLine("     }");
        source.AppendLine("}");
    }


    private static string GetConvertText(Property property1, Property property2)
    {
        if (string.Equals(property1.Type, property2.Type, StringComparison.CurrentCultureIgnoreCase))
            return $"{property1.Name} = value.{property2.Name},";

        return property1.Type.Trim() switch
        {
            "string" or "string?" or "String" => $"{property1.Name} = value.{property2.Name}.ToString(),",
            "char" or "char?" or "Char" => $"{property1.Name} = global::System.Convert.ToChar(value.{property2.Name}),",
            "double" or "double?" or "Double" =>
                $"{property1.Name} = global::System.Convert.ToDouble(value.{property2.Name}),",
            "long" or "long?" or "Int64" =>
                $"{property1.Name} = global::System.Convert.ToInt64(value.{property2.Name}),",
            "float" or "float?" or "Single" =>
                $"{property1.Name} = global::System.Convert.ToSingle(value.{property2.Name}),",
            "int" or "int?" or "Int32" => $"{property1.Name} = global::System.Convert.ToInt32(value.{property2.Name}),",
            "short" or "short?" or "Int16" =>
                $"{property1.Name} = global::System.Convert.ToInt16(value.{property2.Name}),",
            "bool" or "bool?" or "Boolean" =>
                $"{property1.Name} = global::System.Convert.ToBoolean(value.{property2.Name}),",
            "byte" or "byte?" or "Byte" => $"{property1.Name} = global::System.Convert.ToByte(value.{property2.Name}),",
            "Decimal" => $"{property1.Name} = global::System.Convert.ToDecimal(value.{property2.Name}),",
            "DateTime" => $"{property1.Name} = global::System.Convert.ToDateTime(value.{property2.Name}),",
            _ => (property1.TypeHasMapper || property2.TypeHasMapper)
                ? $"{property1.Name} = this._mapper.Map<{property2.FullTypeName}, {property1.FullTypeName}>(value.{property2.Name})!,"
                : string.Empty

            // _ => $"{property1Name} = this._mapper?.TryMap<{property1Type.Trim()}>(value.{property2Name}) ?? default!,"
        };
    }
}