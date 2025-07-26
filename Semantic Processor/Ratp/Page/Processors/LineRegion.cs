using HtmlAgilityPack;
using ProcessLib.Ratp.Page.Processors;

namespace Semantic_Processor.Ratp.Page.Processors;

public class LineRegion
{
    // 段落id
    public int? ParagraphId
    {
        get;
        set;
    }

    private RegionType? _type;
    public RegionType? Type
    {
        get
        {
            return _type ??= GetRegionType();
        }
        set => _type = value;
    }

    private RegionType GetRegionType()
    {
        var decorator = $"<div>{Div}</div>";
        var document = new HtmlDocument();
        document.LoadHtml(decorator);
        var node = document.DocumentNode.SelectSingleNode("//div/div");
        var classStr = node.GetAttributeValue("class", "");
        var classes = classStr.Split(" ").ToList();
        var first = classes.FirstOrDefault();
        if (first == "c")
        {
            return RegionType.Container;
        }
        return first == "t" ? RegionType.Text : RegionType.Other;
    }
    public string? Div
    {
        get;
        set;
    }
}