using HtmlAgilityPack;
using PDF_Translate_Layout_Lib.Ratp.Easy.Mark;

namespace PDF_Translate_Layout_Lib.Natp.Collector;

public static class ScriptsCollector
{
    // ReSharper disable once MemberCanBePrivate.Global
    public static Dictionary<string, string> Collect(string html)
    {
        var result = new Dictionary<string, string>();

        // Load the HTML document
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        // Select all elements with the raptr attribute
        var elements = doc.DocumentNode.SelectNodes("//*[@raptr]");

        foreach (var element in elements)
        {
            // Get the raptr attribute value
            // ReSharper disable once IdentifierTypo
            var raptrValue = element.GetAttributeValue(Rtp.TranslateAttr, "");

            // Get the inner text of the element
            var innerText = element.InnerText;

            // Add the raptr attribute value and inner text to the dictionary
            result[raptrValue] = innerText;
        }

        return result;
    }

    public static async Task<Dictionary<string, string>> CollectFromDiskAsync(string path)
    {
        var html = await File.ReadAllTextAsync(path);
        return Collect(html);
    }
}