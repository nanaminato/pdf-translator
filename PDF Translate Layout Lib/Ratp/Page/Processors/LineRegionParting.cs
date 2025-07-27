using HtmlAgilityPack;

namespace PDF_Translate_Layout_Lib.Ratp.Page.Processors;

public class LineRegionParting
{
    public static List<LineRegion> GetLineRegions(string context)
    {
        var document = new HtmlDocument();
        document.LoadHtml(context);//descendant
        var divNodes = document.DocumentNode.SelectNodes("//div[contains(@class, 'pc')]/child::*").ToList();

        return divNodes.Select(divNode => new LineRegion() { Div = divNode.OuterHtml }).ToList();
    }
}