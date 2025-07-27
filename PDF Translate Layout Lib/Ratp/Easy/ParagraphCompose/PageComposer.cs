using HtmlAgilityPack;
using PDF_Translate_Layout_Lib.Natp.Collector;
using PDF_Translate_Layout_Lib.Natp.Parameters;
using PDF_Translate_Layout_Lib.Ratp.Easy.Breaker;
using PDF_Translate_Layout_Lib.Ratp.Page.Processors;
using PDF_Translate_Layout_Lib.Ratp.Page.Region;
using PDF_Translate_Layout_Lib.Utils.HTML;

namespace PDF_Translate_Layout_Lib.Ratp.Easy.ParagraphCompose;

public class PageComposer
{
    public StyleNode StyleNode
    {
        get;
        set;
    }

    public string PageDoc
    {
        get;
        set;
    }

    public List<LineRegion>? LineRegions
    {
        get;
        set;
    }

    public List<SemanticParagraph>? Paragraphs
    {
        get;
        set;
    }

    public string Outter
    {
        get;
        set;
    }

    private DebugParameter DebugParameter
    {
        get;
    }

    private StyleCollector Collector
    {
        get;
    }

    public int PageId
    {
        get;
    }
    public PageComposer(StyleNode styleNode, string page,DebugParameter parameter,StyleCollector collector,int pageId)
    {
        StyleNode = styleNode;
        PageDoc = page;
        DebugParameter = parameter;
        Collector = collector;
        PageId = pageId;
        Outter = Skeleton();
    }

    // 获取骨架
    private string Skeleton()
    {
        var document = new HtmlDocument();
        document.LoadHtml(PageDoc);
        var select = document.DocumentNode.SelectSingleNode("//div[contains(@class,'pf')]/div");
        if (select == null) throw new Exception();
        select.InnerHtml = "";
        return document.DocumentNode.OuterHtml;
    }
    // 组合成一个页面的完整内容
    public string Compose()
    {
        var skipFirstLine = true;
        List<(int, double, double,double,double)> _scopes = new();
        try
        {
            using var reader = new StreamReader(DebugParameter.ParagraphWidthAssociationFile);

            while (reader.ReadLine() is { } line)
            {
                if (skipFirstLine)
                {
                    skipFirstLine = false;
                    continue; // 忽略第一行
                }

                var parts = line.Split(','); // 根据实际的分隔符分割行内容
                if (parts.Length == 5 && int.TryParse(parts[0], out var intValue) &&
                    double.TryParse(parts[1], out var doubleValue1) &&
                    double.TryParse(parts[2], out var doubleValue2)&&
                    double.TryParse(parts[3], out var doubleValue3)&&
                    double.TryParse(parts[4], out var doubleValue4))
                {
                    _scopes.Add((intValue, doubleValue1, doubleValue2,doubleValue3,doubleValue4));
                }
                else
                {
                    // 处理无法解析的行或错误
                    Console.WriteLine($"Error parsing line: {line}");
                }
            }
        }
        catch (IOException e)
        {
            Console.WriteLine("An error occurred while reading the file: " + e.Message);
        }
        LineRegions = LineRegionParting.GetLineRegions(PageDoc);
        // var breaker = new StepwiseParagraphBreaker();
        var breaker = new OldStepParagraphBreaker();
        Paragraphs = breaker.Breaker(LineRegions,StyleNode,DebugParameter,_scopes);
        var process = new SpanProcessor(DebugParameter,Collector,PageId,_scopes);
        var main = process.Process(Paragraphs, StyleNode);

        // main代表的是一个页面的内部段落
        var document = new HtmlDocument();
        document.LoadHtml(Outter);
        var select = document.DocumentNode.SelectSingleNode("//div[contains(@class,'pf')]/div");
        select.InnerHtml = main;
        return document.DocumentNode.OuterHtml;
    }
}