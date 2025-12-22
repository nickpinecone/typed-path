using Microsoft.CodeAnalysis;

namespace SourceGen;

public static class AttributeHelper
{
    public static string? GetArgumentValue(this AttributeData attribute, string argumentName)
    {
        foreach (var namedArg in attribute.NamedArguments)
        {
            if (namedArg.Key == argumentName)
            {
                return namedArg.Value.Value?.ToString();
            }
        }

        var constructorParams = attribute.AttributeConstructor?.Parameters;

        if (constructorParams is not null)
        {
            for (var i = 0; i < constructorParams.Value.Length; i++)
            {
                if (constructorParams.Value[i].Name == argumentName.ToLower() &&
                    i < attribute.ConstructorArguments.Length)
                {
                    return attribute.ConstructorArguments[i].Value?.ToString();
                }
            }
        }

        return null;
    }
}