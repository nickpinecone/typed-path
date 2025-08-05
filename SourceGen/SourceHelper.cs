using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;

namespace SourceGen;

public static class SourceHelper
{
    public static string Attribute =>
        $$"""
        namespace TypedPath;

        {{GeneratedCodeAttribute}}
        [AttributeUsage(AttributeTargets.Class)]
        public class TypedPathAttribute : Attribute
        {
            public string Path { get; }

            public TypedPathAttribute(string path)
            {
                Path = path;
            }
        }
        """;

    public static string Interface =>
        $$"""
        namespace TypedPath;

        {{GeneratedCodeAttribute}}
        public interface ITypedPath
        {
            public static abstract string Wrap(string path);
        }
        """;

    private static string GeneratedCodeAttribute =>
        "[global::System.CodeDom.Compiler.GeneratedCode(\"TypedPath\", \"1.0.0\")]";

    public static string GenerateTyped(string name, string attrPath, string namespaceName,
        ImmutableArray<AdditionalText> files)
    {
        var builder = new StringBuilder();

        if (!string.IsNullOrEmpty(namespaceName))
        {
            builder.AppendLine($"namespace {namespaceName};");
            builder.AppendLine("");
        }

        builder.AppendLine(GeneratedCodeAttribute);
        builder.AppendLine($"public partial class {name}");
        builder.AppendLine("{");

        foreach (var file in files)
        {
            var filename = Path.GetFileNameWithoutExtension(file.Path).ToPascalCase();
            var split = file.Path.Split([attrPath], StringSplitOptions.None);

            if (split.Length < 2)
            {
                continue;
            }

            var relativePath = split.Last();
            var paths = relativePath.Split([Path.DirectorySeparatorChar], StringSplitOptions.RemoveEmptyEntries);
            var final = $"{name}{relativePath}";

            GenerateNested(builder, paths, 0, filename, final);
        }

        builder.AppendLine("}");

        return builder.ToString();
    }

    private static void GenerateNested(StringBuilder builder, string[] paths, int current, string name, string value)
    {
        if (current >= paths.Length - 1)
        {
            builder.Append(CalculateIndent(current));
            builder.AppendLine($"public static readonly string {name} = Wrap(\"{value}\");");
            return;
        }

        builder.Append(CalculateIndent(current));
        builder.AppendLine($"public static partial class {paths[current]}");

        builder.Append(CalculateIndent(current));
        builder.AppendLine("{");

        GenerateNested(builder, paths, current + 1, name, value);

        builder.Append(CalculateIndent(current));
        builder.AppendLine("}");
    }

    private static string CalculateIndent(int current)
    {
        return string.Join("", Enumerable.Repeat("    ", current + 1));
    }
}