using System.Text;
using HtmlAgilityPack;

namespace PDF_Translate_Layout_Lib.Natp.Appender;

public abstract class PreviewAppender
{
    public static string Append(string html, Dictionary<string, string> fx2Color)
    {
        var styleBuilder = new StringBuilder();
        var colorClassMap = new Dictionary<string, string>();

        var colorIndex = 1;
        foreach (var (key, colorCode) in fx2Color)
        {
            var className = "preview_color" + colorIndex;

            styleBuilder.AppendLine($".{className} {{");
            styleBuilder.AppendLine($"    color: {colorCode} !important;");
            styleBuilder.AppendLine("}");

            colorClassMap[key] = className;

            colorIndex++;
        }

        var style = styleBuilder.ToString();
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        FontRecognitionColorAppender.Append(doc, colorClassMap);
        StyleNodeAppender.Append(doc, style);

        return doc.DocumentNode.OuterHtml;
    }
}