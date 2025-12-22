using System;
using System.IO;

namespace TypedPath.Sample;

[TypedPath("Assets")]
public partial class Assets : ITypedPath
{
    public static string Wrap(string path)
    {
        return "Prefix/" + path;
    }
}

[TypedPath("Assets/SubFolder")]
public partial class SubFolder : ITypedPath
{
    public static string Wrap(string path)
    {
        return Path.GetFileName(path).Replace(".svg", ".png");
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine(Assets.SubFolder.NestedFolder.SuperNested);
        Console.WriteLine(Assets.SubFolder.Logo);
        Console.WriteLine(SubFolder.Logo);
    }
}