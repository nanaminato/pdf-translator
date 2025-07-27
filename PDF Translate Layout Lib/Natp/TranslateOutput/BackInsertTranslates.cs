using System.Net;
using HtmlAgilityPack;
using PDF_Translate_Layout_Lib.Natp.Manipulator;

namespace PDF_Translate_Layout_Lib.Natp.TranslateOutput;

public class BackInsertTranslates
{
    public static string BackInsert(string html,int from,int to, List<MiniParam> translates)
    {
        html = HtmlManipulator.ProcessSpliceWithRange(html,from,to);
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        // Select all elements with the raptr attribute
        var elements = doc.DocumentNode.SelectNodes("//*[@raptr]");

        foreach (var element in elements)
        {
            // Get the raptr attribute value
            var raptrValue = element.GetAttributeValue("raptr", "");

            // Parse P1 and P2 values from the raptr attribute
            var raptrParts = raptrValue.Split('_');
            if (raptrParts.Length == 2 && int.TryParse(raptrParts[0], out var p1) && int.TryParse(raptrParts[1], out var p2))
            {
                // Find the corresponding translation
                var translation = translates.FirstOrDefault(t => t.P1 == p1 && t.P2 == p2);

                if (translation != null)
                {
                    // Replace the inner text of the element with the translation
                    element.InnerHtml = WebUtility.HtmlEncode(translation.P3);
                }
            }
        }

        return doc.DocumentNode.OuterHtml;
    }
}