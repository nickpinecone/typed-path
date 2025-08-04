using TypedPath;

namespace Sample;

[TypedPath("Assets")]
public static partial class Assets
{
}

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine(Assets.SubFolder.NestedFolder.SuperNested);
    }
}