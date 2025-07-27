using System.Text;
using HtmlAgilityPack;
using PDF_Translate_Layout_Lib.Natp.Collector;
using PDF_Translate_Layout_Lib.Natp.Parameters;
using PDF_Translate_Layout_Lib.Ratp.Easy.Extension;
using PDF_Translate_Layout_Lib.Ratp.Easy.Mark;
using PDF_Translate_Layout_Lib.Ratp.Easy.Ref;
using PDF_Translate_Layout_Lib.Ratp.Easy.Select;
using PDF_Translate_Layout_Lib.Ratp.Page.Region;
using PDF_Translate_Layout_Lib.Utils.HTML;

namespace PDF_Translate_Layout_Lib.Ratp.Easy.ParagraphCompose;

public class SpanProcessor
{
    private DebugParameter DebugParameter
    {
        get;
    }

    public ExecutionTimeParameter EtParameter
    {
        get;
    }
    private StyleCollector Collector
    {
        get;
    }

    private int PageId
    {
        get;
    }

    private readonly List<(int, double, double,double,double)> _scopes = new();

    public SpanProcessor(DebugParameter debugParameter,StyleCollector collector, int pageId,List<(int, double, double,double,double)> scopes)
    {
        DebugParameter = debugParameter;
        EtParameter = DebugParameter.Build();
        Collector = collector;
        PageId = pageId;
        _scopes = scopes;
    }
    public string Process(List<SemanticParagraph> paragraphs, StyleNode styleNode)
    {
        var builder = new StringBuilder();
        var refI = new Integer(0);
        foreach (var pa in paragraphs)
        {
            builder.Append(Process(pa,styleNode,refI));
        }
        return builder.ToString();
    }

    private static string Raw(SemanticParagraph semanticParagraph)
    {
        var builder = new StringBuilder();
        builder.Append('\n');
        foreach (var line in semanticParagraph.LineRegions!)
        {
            builder.Append(line.Div);
        }

        builder.Append('\n');

        return builder.ToString();
    }

    // 对于一个段落的处理
    private string Process(SemanticParagraph paragraph, StyleNode styleNode,Integer refInt)
    {
        // FontFamily 为空时，代表不是常规的段落，使用""来按照既定规则进行读取，返回false
        if (EtParameter.IsMain(paragraph.FontFamily??"",paragraph.FontSize??""))
        {
            if (paragraph.LineRegions!.Count < 1) return "";

            refInt.Value++;
            const bool useTop = false;
            var firstNode = useTop?DivHelpSelector.Select(
                paragraph.LineRegions.First().Div!, "div"):DivHelpSelector.Select(
                paragraph.LineRegions.Last().Div!, "div");
            double max = 0;
            foreach (var attrValue in paragraph.LineRegions.Select(region => DivHelpSelector.Select(
                         region.Div!, "div")).Select(node => node.GetAttributeValue("rid", "0")))
            {
                var parse = int.TryParse(attrValue, out var real);
                var rwidth = _scopes.Where(s => s.Item1 == real).Select(s=>s.Item2).ToList();
                if (parse && rwidth.Count>0)
                {
                    if (max < rwidth[0])
                    {
                        max = rwidth[0];
                    }
                }
            }
            var builder = new StringBuilder();
            var document = new HtmlDocument();
            document.LoadHtml(Raw(paragraph));
            var nodes = document.DocumentNode.SelectNodes("//div[contains(@class,'t')]");
            var tab = 0.0;
            if (nodes.Count >= 2)
            {
                // 初步检查的回滚值，未考虑后果
                var x1 = styleNode.Lefts.TryGetValue(Extractor.ExtractorX(nodes[0].GetAttribute("class")) ?? string.Empty,out var dx1);
                var x2 = styleNode.Lefts.TryGetValue(Extractor.ExtractorX(nodes[1].GetAttribute("class")) ?? string.Empty,out var dx2);
                if (x1 && x2)
                {
                    tab = dx2??0 - dx1??0;
                }
            }
            foreach (var node in nodes)
            {
                ExtractTextFromNode(node);
            }

            firstNode.InnerHtml = builder.ToString();
            var ca = firstNode.GetAttribute("class");
            if (paragraph.LineRegions.Count >= 2)
            {
                var secondNode = DivHelpSelector.Select(paragraph.LineRegions[1].Div!, "div");
                var x = Extractor.ExtractorX(secondNode.GetAttribute("class"));
                var xf = Extractor.ExtractorX(ca);
                if (x != null&&xf!=null)
                {
                    ca = ca.Replace(xf, x);
                }
            }
            var h = Extractor.ExtractorHeight(ca);
            if (h != null)
            {
                ca = ca.Replace(h, "");
            }


            var topFix = "";
            if (useTop)
            {
                // use top rather bottom;
                var rid = firstNode.GetAttribute("rid");
                var tryParse = int.TryParse(rid, out var rRid);
                if (tryParse)
                {
                    var fixTop = _scopes.FirstOrDefault(s => s.Item1 == rRid);
                    if (fixTop.Item1 != 0)
                    {
                        topFix = $"top: {fixTop.Item5}px;";
                        var b = Extractor.ExtractorY(ca);
                        if (b != null)
                        {
                            ca = ca.Replace(b, "");
                        }
                    }

                }
            }

            firstNode.Attributes.Remove("class");
            firstNode.Attributes.Add("class",ca+" pw");
            var tabStr = (tab - 0) > 0.1 ? $"text-indent: {tab}px;" : "";
            // 标记打印优化
            var appender = paragraph.LineRegions.Count > 1?"text-wrap: wrap;":"";
            firstNode.Attributes.Add("style",$"width: {max}px;" +
                                             $"{appender}{topFix}" +
                                             $"word-spacing: 0px;  {tabStr}");
            // 标记
            firstNode.Attributes.Add(Rtp.TranslateAttr,$"{PageId}_{refInt.Value}");
            return firstNode.OuterHtml;
            void ExtractTextFromNode(HtmlNode nodeX)
            {
                if (nodeX == null) throw new ArgumentNullException(nameof(nodeX));
                if (nodeX == null) throw new ArgumentNullException(nameof(nodeX));
                if (nodeX.NodeType == HtmlNodeType.Text)
                {
                    var text = nodeX.InnerHtml;
                    if (!string.IsNullOrEmpty(text))
                    {
                        builder.Append(text);
                    }
                }
                else if (nodeX.NodeType == HtmlNodeType.Element)
                {
                    if (nodeX.Name == "span")
                    {
                        var className = nodeX.GetAttribute("class");
                        var innerText = nodeX.InnerText;
                        var ff = Extractor.ExtractorFontFamily(className);
                        builder.Append(innerText);
                    }
                    else
                    {
                        foreach (var childNode in nodeX.ChildNodes)
                        {
                            ExtractTextFromNode(childNode);
                        }
                    }
                }
            }
        }
        if (EtParameter.IsDontTranslate(paragraph.FontFamily??"",paragraph.FontSize??""))
        {
            return Raw(paragraph);
            // return @"<pre><code>{}</code></pre>".Replace("{}",Raw(paragraph));
        }

        return Raw(paragraph);




    }


}