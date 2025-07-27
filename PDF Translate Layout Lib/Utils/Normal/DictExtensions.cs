namespace PDF_Translate_Layout_Lib.Utils.Normal;

public static class DictExtensions
{
    public static void ListElements(this Dictionary<string, double?> dict)
    {
        foreach (var (key,value) in dict)
        {
            Console.WriteLine($"{key}: {value}");
        }
    }
    public static void ListElements(this Dictionary<string, string?> dict)
    {
        foreach (var (key,value) in dict)
        {
            Console.WriteLine($"{key}: {value}");
        }
    }
}