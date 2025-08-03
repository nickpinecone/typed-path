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

    public static string GenerateClass(string name)
    {
        return $@"
namespace Sample;

public static partial class {name}
{{
    public const string CoolImage = ""/somepath"";
}}
               ";
    }
}