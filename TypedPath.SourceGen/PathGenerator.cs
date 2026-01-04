using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace TypedPath.SourceGen;

[Generator]
public class PathGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
            "TypedPathAttribute.g.cs",
            SourceText.From(SourceHelper.Attribute, Encoding.UTF8))
        );

        context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
            "TypedPathInterface.g.cs",
            SourceText.From(SourceHelper.Interface, Encoding.UTF8))
        );

        var files = context.AdditionalTextsProvider.Collect();

        var classes = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                "TypedPath.TypedPathAttribute",
                predicate: static (syntax, _) => syntax is ClassDeclarationSyntax,
                transform: static (ctx, _) => ctx.SemanticModel.GetDeclaredSymbol(ctx.TargetNode)
            )
            .Where(static m => m is not null)
            .Select(static (m, _) => m!)
            .Combine(files);

        context.RegisterSourceOutput(classes, (ctx, source) => Execute(source, ctx));
    }

    private static void Execute(
        (ISymbol ClassSymbol, ImmutableArray<AdditionalText> Files) tuple,
        SourceProductionContext context
    )
    {
        var name = tuple.ClassSymbol.Name;
        var namespaceName = tuple.ClassSymbol.ContainingNamespace.ToDisplayString();

        var attr = tuple.ClassSymbol.GetAttributes()
            .First(a => a.AttributeClass?.Name == "TypedPathAttribute");

        context.AddSource(
            $"TypedPath.{name}.g.cs",
            SourceText.From(SourceHelper.GenerateTyped(name, attr, namespaceName, tuple.Files), Encoding.UTF8)
        );
    }
}