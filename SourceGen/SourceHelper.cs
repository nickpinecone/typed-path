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

    public static string GenerateClass(string name, ImmutableArray<AdditionalText> files)
    {
        var builder = new StringBuilder();
        builder.AppendLine("namespace Sample;");
        builder.AppendLine($"public static partial class {name}");
        builder.AppendLine("{");

        foreach (var file in files)
        {
            var filename = Path.GetFileNameWithoutExtension(file.Path).ToPascalCase();
            var split = file.Path.Split([name], StringSplitOptions.None);
            var relative = $"{name}{split.Last()}";

            builder.AppendLine($"   public const string {filename} = \"{relative}\";");
        }

        builder.AppendLine("}");

        return builder.ToString();
    }

    public static string ToPascalCase(this string text)
    {
        var builder = new StringBuilder();
        var flag = true;

        for (var index = 0; index < text.Length; ++index)
        {
            var c = text[index];

            if (char.IsLetterOrDigit(c))
            {
                if (flag)
                {
                    builder.Append(char.ToUpperInvariant(c));
                    flag = false;
                }
                else
                {
                    builder.Append(index >= text.Length - 1 || !char.IsUpper(c) || !char.IsLower(text[index + 1])
                        ? char.ToLowerInvariant(c)
                        : c);
                }
            }
            else
            {
                flag = true;
            }

            if (index < text.Length - 1 && char.IsLower(text[index]) && char.IsUpper(text[index + 1]))
            {
                flag = true;
            }
        }

        return builder.ToString();
    }
}