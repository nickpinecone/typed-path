using TypedPath;

namespace Sample;

[TypedPath("Assets")]
public static partial class Assets
{
}

[TypedPath("Assets/SubFolder")]
public static partial class SubFolder
{
}

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine(Assets.SubFolder.NestedFolder.SuperNested);
        Console.WriteLine(SubFolder.NestedFolder.SuperNested);
    }
}