using System.Text;

namespace PDF_Translator_Utils.Utils.Normal;

public static class ListExtensions
{
    public static string ListElements(this IEnumerable<string> list,string app="\n")
    {
        var builder = new StringBuilder();
        foreach (var ele in list)
        {
            builder.Append(ele + app);
        }

        Console.WriteLine(builder);
        return builder.ToString();
    }
}