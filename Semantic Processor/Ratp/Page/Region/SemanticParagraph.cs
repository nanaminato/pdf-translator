using Semantic_Processor.Ratp.Page.Processors;

namespace Semantic_Processor.Ratp.Page.Region;

public class SemanticParagraph
{
    public int Count => LineRegions!.Count;
    // 语义段落
    public List<LineRegion>? LineRegions
    {
        get;
        set;
    } = new();

    public bool IsEmpty => LineRegions?.Count == 0;

    public string? FontFamily
    {
        get;
        set;
    }

    public string? FontSize
    {
        get;
        set;
    }

    public override string ToString()
    {
        var res = $"SemanticParagraph: {GetHashCode()}\n";
        res = LineRegions!.Aggregate(res, (current, lineRegion) => current + $"line: {lineRegion.Div}\n");

        res += $"font-family: {FontFamily}\n";
        return res;
    }

    public void Add(LineRegion line)
    {
        LineRegions!.Add(line);
    }

    public void Build()
    {
        // 填充font-family ,font size
    }
}