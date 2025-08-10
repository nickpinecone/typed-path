# Typed Path
.NET source generator for strongly typed file paths

## Install
Install the package from nuget
```sh
dotnet add package ...
```

## Configuration
The `Sample` project includes the full example

First add the folder to .csproj as AdditionalFiles
```xml
<ItemGroup>
    <AdditionalFiles Include="Assets/**" />
</ItemGroup>
```

Then define a partial class with `TypedPath` attribute and `ITypedPath` interface
```csharp
[TypedPath("Assets")]
public partial class Assets : ITypedPath
{
    public static string Wrap(string path)
    {
        return path;
    }
}
```

After that you can just reference the path you need
```csharp
> Console.WriteLine(Assets.SubFolder.NestedFolder.SuperNested);

Assets/SubFolder/NestedFolder/SuperNested.json
```

## Note for Rider
Related to [RIDER-75959](https://youtrack.jetbrains.com/issue/RIDER-75959/Rider-doesnt-trigger-IncrementalSourceGenerator-when-changing-AdditionalFiles)

While the source generator works with most file types (`.json`, `.png`, etc.),  
Rider's IntelliSense may not detect changes with `.txt` and potentially other files.

**Builds will still work correctly** (this is purely an IDE issue)  