using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

        var classes = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                "TypedPath.TypedPathAttribute",
                predicate: static (syntax, _) => syntax is ClassDeclarationSyntax,
                transform: static (ctx, _) => (ClassDeclarationSyntax)ctx.TargetNode
            )
            .Where(static m => m is not null);

        context.RegisterSourceOutput(classes, static (ctx, source) => Execute(source, ctx));
    }

    private static void Execute(ClassDeclarationSyntax classSyntax, SourceProductionContext context)
    {
        context.AddSource($"TypedPath.{classSyntax.Identifier.Text}.g.cs",
            SourceText.From(SourceHelper.GenerateClass(classSyntax.Identifier.Text), Encoding.UTF8));
    }
}