using System.Text;
using HtmlAgilityPack;
using PDF_Translate_Layout_Lib.Natp.Parameters;
using PDF_Translate_Layout_Lib.Ratp.Easy.Extension;
using PDF_Translate_Layout_Lib.Ratp.Easy.Select;
using PDF_Translate_Layout_Lib.Ratp.Page.Processors;
using PDF_Translate_Layout_Lib.Ratp.Page.Region;
using PDF_Translate_Layout_Lib.Utils.HTML;

namespace PDF_Translate_Layout_Lib.Ratp.Easy.Breaker;

public class MixedParagraphBreaker: ParagraphBreaker
{
    public override List<SemanticParagraph> Breaker(List<LineRegion> regions,StyleNode styleNode,DebugParameter parameter,List<(int, double, double,double,double)> scopes)
    {
        // scope
        // id width height left top
        var paragraphs = new List<SemanticParagraph>();
        var semanticParagraph = new SemanticParagraph();
        var companion = new Companion();
        foreach (var line in regions)
        {
            if (semanticParagraph.IsEmpty)
            {
                AddFirst(semanticParagraph,companion, line, styleNode,parameter,scopes);
                continue;
            }
            if (companion.Finish||!IsAssociation(semanticParagraph,companion, line, styleNode,parameter,scopes))
            {
                semanticParagraph.Build();
                paragraphs.Add(semanticParagraph);
                semanticParagraph = new SemanticParagraph();
                companion = new Companion();
            }
            else
            {
                semanticParagraph.Add(line);
            }
        }

        return paragraphs;
    }

    private static void AddFirst(SemanticParagraph semanticParagraph,Companion companion, LineRegion line, StyleNode styleNode, DebugParameter parameter, List<(int, double, double, double, double)> scopes)
    {
        // 引入 funny 和 finish
        // 如果是某些样式的元素，直接标识该段落已经结束，比如 该元素没有 y，以及字体属性
        // 如果在funny 开头表，就设置funny 为true
        var node = DivHelpSelector.Select(line.Div!, "/descendant::*");
        var classStr = node.GetAttribute("class");
        var fontFamily = Extractor.ExtractorFontFamily(classStr);
        var fontSize = Extractor.ExtractorFontSize(classStr);
        if (fontFamily == null || fontSize == null)
        {
            semanticParagraph.Add(line);
            companion.Finish = true;
            return;
        }
        var left = Extractor.ExtractorX(classStr);
        var bottom = Extractor.ExtractorY(classStr)??"";
        var btt = styleNode.Bottoms.TryGetValue(bottom, out var btn);
        if (!btt)
        {
            semanticParagraph.Add(line);
            companion.Finish = true;
        }
        var et = parameter.Build();
        if (et.IsFunnyStart(fontFamily, fontSize))
        {
            companion.Funny = true;
            companion.ContainFunny = true;
        }
        semanticParagraph.Add(line);
    }
    private static bool IsAssociation(SemanticParagraph paragraph,Companion companion, LineRegion line,StyleNode styleNode,DebugParameter parameter,List<(int, double, double,double,double)> scopes)
    {
        var node = DivHelpSelector.Select(line.Div!, "/descendant::*");
        var classStr = node.GetAttribute("class");
        var fontFamily = Extractor.ExtractorFontFamily(classStr);
        var fontSize = Extractor.ExtractorFontSize(classStr);
        if (fontFamily == null || fontSize == null)
        {
            return false;
        }
        var left = Extractor.ExtractorX(classStr);
        var bottom = Extractor.ExtractorY(classStr)??"";
        var btt = styleNode.Bottoms.TryGetValue(bottom, out var btn);
        if (!btt)
        {
            // 不存在bottom ,
            return false;
        }

        var et = parameter.Build();
        if (et.IsFunnyStart(fontFamily, fontSize))// 在funny表中
        {
            return true;
        }
        // 新的一行不是funny
        if (companion.Funny)
        {
            // !paragraph.LineRegions!.Any(region =>
            // {
            //     var select = DivHelpSelector.Select(region.Div!, "/descendant::*");
            //     var attribute = select.GetAttribute("class");
            //     var extractorFontFamily = Extractor.ExtractorFontFamily(attribute)??"";
            //     var extractorFontSize = Extractor.ExtractorFontSize(attribute)??"";
            //     return !et.IsFunnyStart(extractorFontFamily, extractorFontSize);
            // })
            // ! 任何一个不是
            // 全都是 funnyStart
            companion.Funny = false;
            return true;
        }//段落的最后一个不是funny
        if (DiffAvailable(paragraph,companion, line, styleNode, parameter, scopes))
        {
            return true;
        }
        return false;
    }

    private static bool DiffAvailable(SemanticParagraph paragraph,Companion companion, LineRegion line,StyleNode styleNode,DebugParameter parameter,List<(int, double, double,double,double)> scopes)
    {
        if (companion.ContainFunny)
        {
            // n* funny,one line   newline
            var ll1 = paragraph.LineRegions!.Last();
            var ll2 = paragraph.LineRegions!.Where(l => l != ll1)
                .ToList().Last();
            var c1 = DivHelpSelector.Select(ll1.Div!, "/descendant::*").GetAttribute("class");
            var c2 = DivHelpSelector.Select(ll2.Div!, "/descendant::*").GetAttribute("class");;
            var c3 = DivHelpSelector.Select(line.Div!, "/descendant::*").GetAttribute("class");;
            // 首先判断 x， 然后判断y
            var x1 = double.Parse(Extractor.ExtractorX(c1)!);
            var x3 = double.Parse(Extractor.ExtractorX(c3)!);
            // n - l1
            if (Math.Abs(x3 - x1) / Math.Abs(x1) > 0.05)
            {
                return false;
            }
            var y1 = double.Parse(Extractor.ExtractorY(c1)!);
            var y2 = double.Parse(Extractor.ExtractorY(c2)!);
            var y3 = double.Parse(Extractor.ExtractorY(c3)!);
            // n - l1 -(l1-l2)
            if(Math.Abs(y3-y1-(y1-y2))/Math.Abs(y2-y1)>0.05)
            {
                return false;
            }
            return false;
        }
        // 1-n line   newline
        if (paragraph.Count == 1)
        {
            var ll1 = paragraph.LineRegions!.Last();
            var c1 = DivHelpSelector.Select(ll1.Div!, "/descendant::*").GetAttribute("class");
            var c3 = DivHelpSelector.Select(line.Div!, "/descendant::*").GetAttribute("class");;
            // 首先判断 x， 然后判断y
            double x1, x3;
            try
            {
                x1 = double.Parse(Extractor.ExtractorX(c1)!);
                x3 = double.Parse(Extractor.ExtractorX(c3)!);
            }
            catch (Exception)
            {
                return false;
            }
            // n - l1
            if (Math.Abs(x3 - x1) / Math.Abs(x1) > 0.05)
            {
                return false;
            }
            // 宽容的字体

        }
        else
        {
            //x
            //y
            // 宽容的字体
        }
        return false;
    }

    // 不在funny表中，这时就需要重新来使用其他元素来判断

    // 使用 x,y,更替的ff,fs 来判断
    // width 等
    // 判断y时，使用行之间的差值的 差别率
    // 使用y时可参考上一行是否是满行  满行，和最大行宽度之间的差别率
    void ExtractTextFromNode(StringBuilder builder,HtmlNode nodeX)
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
                    ExtractTextFromNode(builder,childNode);
                }
            }
        }
    }

    private class Companion
    {
        public bool Funny
        {
            get;
            set;
        } = false;

        public bool ContainFunny = false;

        public bool Finish
        {
            get;
            set;
        } = false;
    }
}