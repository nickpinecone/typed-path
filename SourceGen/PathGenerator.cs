using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace SourceGen;

[Generator]
public class PathGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
            "TypedPathAttribute.g.cs",
            SourceText.From(SourceHelper.Attribute, Encoding.UTF8))
        );

        var files = context.AdditionalTextsProvider.Collect();

        var classes = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                "TypedPath.TypedPathAttribute",
                predicate: static (syntax, _) => syntax is ClassDeclarationSyntax,
                transform: static (ctx, _) => (ClassDeclarationSyntax)ctx.TargetNode
            )
            .Where(static m => m is not null)
            .Combine(files);

        context.RegisterSourceOutput(classes, (ctx, source) => Execute(source, ctx));
    }

    private static void Execute((ClassDeclarationSyntax ClassSyntax, ImmutableArray<AdditionalText> Files) tuple,
        SourceProductionContext context)
    {
        context.AddSource($"TypedPath.{tuple.ClassSyntax.Identifier.Text}.g.cs",
            SourceText.From(SourceHelper.GenerateClass(tuple.ClassSyntax.Identifier.Text, tuple.Files), Encoding.UTF8));
    }
}