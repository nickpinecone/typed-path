using System.Text;

namespace SourceGen;

public static class CaseConverter
{
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