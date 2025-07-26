using PDF_Translator_Utils.Utils.HTML;
using Semantic_Processor.Natp.Parameters;
using Semantic_Processor.Ratp.Easy.Extension;
using Semantic_Processor.Ratp.Easy.Select;
using Semantic_Processor.Ratp.Page.Processors;
using Semantic_Processor.Ratp.Page.Region;

namespace Semantic_Processor.Ratp.Easy.Breaker;


// old, 有些改良的空间
public class StepwiseParagraphBreaker: ParagraphBreaker
{
    public override List<SemanticParagraph> Breaker(List<LineRegion> regions, StyleNode styleNode,
        DebugParameter parameter, List<(int, double, double, double, double)> scopes)
    {
        var paragraphs = new List<SemanticParagraph>();
        // ReSharper disable IdentifierTypo
        var lastff = "";
        var lastx = "";
        var lastfs = "";
        double lastbt = 0;
        var semanticParagraph = new SemanticParagraph();
        var companion = new Companion();
        foreach (var region in regions)
        {
            var node = DivHelpSelector.Select(region.Div!, "/descendant::*");
            var nstr = node.GetAttribute("class");
            var x = Extractor.ExtractorX(nstr);
            var ff = Extractor.ExtractorFontFamily(nstr);
            var fs = Extractor.ExtractorFontSize(nstr);
            var key = Extractor.ExtractorY(nstr)??"";
            var btt = styleNode.Bottoms.TryGetValue(key, out var btn);
            double bt = 0;
            if (btt) bt = btn??0;
            // 新旧不同存在   并且新的不是不翻译的，  并且 新的里面包含旧的样式（实际上是旧的）
            if ((ff != lastff || fs != lastfs))
            {
                // maybe break;
                if (!parameter.Build().IsDontTranslate(ff ?? "", fs ?? ""))
                {
                    if (!SelectTrue(region.Div!, ff ?? "", fs ?? ""))
                    {
                        fbreak();
                        continue;
                    }
                }

            }

            void fbreak()
            {
                // ff break;
                if (semanticParagraph.LineRegions!.Count > 0)
                {
                    Console.WriteLine($"break {region.Div}");
                    paragraphs.Add(semanticParagraph);
                    semanticParagraph = new SemanticParagraph();
                    companion = new Companion();
                    semanticParagraph.LineRegions!.Add(region);
                    semanticParagraph.FontFamily = ff;
                    semanticParagraph.FontSize = fs;
                    lastfs = fs;
                    lastff = ff;
                    lastx = x;
                    lastbt = bt;
                    // continue;
                }
            }
            if (x != lastx)
            {
                companion.x++;
                if (companion.x >= 2||Math.Abs(lastbt-bt)>20)
                {
                    if (semanticParagraph.LineRegions!.Count > 0)
                    {
                        //same to before
                        paragraphs.Add(semanticParagraph);
                        semanticParagraph = new SemanticParagraph();
                        companion = new Companion();
                        semanticParagraph.LineRegions!.Add(region);
                        semanticParagraph.FontFamily = ff;
                        semanticParagraph.FontSize = fs;
                        lastfs = fs;
                        lastff = ff;
                        lastx = x;
                        lastbt = bt;
                        continue;
                    }
                }
            }

            companion.x = 2;// 神之一手，
            semanticParagraph.FontFamily = ff;
            semanticParagraph.FontSize = fs;
            lastfs = fs;
            lastff = ff;
            lastx = x;
            lastbt = bt;
            semanticParagraph.LineRegions!.Add(region);
        }

        return paragraphs;
    }

    private static bool SelectTrue(string line,string ff,string fs)
    {
        return InnerClassSelector.Select(line).Any(tuple => tuple.Item1 == ff && tuple.Item2 == fs);
    }

    private class Companion
    {
        public int x;
    }

}