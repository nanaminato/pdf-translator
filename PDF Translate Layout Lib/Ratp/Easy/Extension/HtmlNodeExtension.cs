using HtmlAgilityPack;

namespace PDF_Translate_Layout_Lib.Ratp.Easy.Extension;

public static class HtmlNodeExtension
{
    public static string GetAttribute(this HtmlNode node,string attr)
    {
        return node.GetAttributeValue(attr, "");
    }
}