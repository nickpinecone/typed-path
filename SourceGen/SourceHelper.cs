using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;

namespace SourceGen;

public static class SourceHelper
{
    public static string Attribute =>
        """
        namespace TypedPath;

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

    public static string GenerateClass(string name, string pathValue, string namespaceName,
        ImmutableArray<AdditionalText> files)
    {
        var builder = new StringBuilder();
        if (!string.IsNullOrEmpty(namespaceName))
        {
            builder.AppendLine($"namespace {namespaceName};");
            builder.AppendLine("");
        }

        builder.AppendLine($"public static partial class {name}");
        builder.AppendLine("{");

        foreach (var file in files)
        {
            var filename = Path.GetFileNameWithoutExtension(file.Path).ToPascalCase();
            var split = file.Path.Split([pathValue], StringSplitOptions.None);
            var path = split.Last();

            var subFolders = path.Split([Path.DirectorySeparatorChar], StringSplitOptions.RemoveEmptyEntries);
            var relative = $"{name}{split.Last()}";

            GenerateNested(builder, subFolders, 0, filename, relative);
        }

        builder.AppendLine("}");

        return builder.ToString();
    }

    private static void GenerateNested(StringBuilder builder, string[] paths, int current, string name, string value)
    {
        if (current >= paths.Length - 1)
        {
            builder.AppendLine($"   public const string {name} = \"{value}\";");
            return;
        }

        builder.AppendLine($"public static partial class {paths[current]}");
        builder.AppendLine("{");

        GenerateNested(builder, paths, current + 1, name, value);

        builder.AppendLine("}");
    }
}