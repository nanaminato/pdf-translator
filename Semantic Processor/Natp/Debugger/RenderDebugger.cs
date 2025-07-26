using HtmlAgilityPack;
using ProcessLib.Ratp.Page.Processors;
using Semantic_Processor.Natp.Appender;
using Semantic_Processor.Natp.Collector;
using Semantic_Processor.Natp.Parameters;
using Semantic_Processor.Ratp.Easy;
using Semantic_Processor.Ratp.Easy.ParagraphCompose;
using Semantic_Processor.Ratp.Easy.Select;
using Semantic_Processor.Ratp.Page.Processors;

namespace Semantic_Processor.Natp.Debugger;

public abstract class RenderDebugger
{
    public static void Process(Composer composer,DebugParameter parameter,string releasePath)
    {
        var document = new HtmlDocument();
        var styleNode = composer.StyleNode;
        document.LoadHtml(composer.MainContainer);
        var pages = PageParting.GetPages(document);//分页

        var pageHtmlList = new List<string>();
        var collector = new StyleCollector();
        foreach (var page in pages)
        {
            var pfx = DivHelpSelector.Select(page.Document!, "div");//选择根节点,pf(x)
            var pfxText = pfx.OuterHtml;
            var pxc = new PageComposer(styleNode, pfxText,parameter,collector,page.PageId);// p(x) composer
            var pageHtml = pxc.Compose();
            pageHtmlList.Add(pageHtml);
        }
        var outHtml = composer.OutHtmlOneStep(pageHtmlList);
        var styleText = collector.BuildStyleText();// 构建收集的样式
        var outWithStyle = StyleNodeAppender.Append(outHtml, styleText);//添加收集的样式
        File.WriteAllText($"{releasePath}",
            $"{outWithStyle}");
    }
}