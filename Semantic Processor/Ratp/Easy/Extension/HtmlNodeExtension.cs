using HtmlAgilityPack;

namespace Semantic_Processor.Ratp.Easy.Extension;

public static class HtmlNodeExtension
{
    public static string GetAttribute(this HtmlNode node,string attr)
    {
        return node.GetAttributeValue(attr, "");
    }
}