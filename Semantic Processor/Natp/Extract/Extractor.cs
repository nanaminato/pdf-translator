using HtmlAgilityPack;
using Semantic_Processor.Ratp.Easy;

namespace Semantic_Processor.Natp.Extract;

public static class Extractor
{
    public static Composer Extract(string path)
    {
        var docText = File.ReadAllText(path);
        // var html = new HtmlDocument();
        // html.LoadHtml(docText);
        // var scriptNodes = html.DocumentNode.SelectNodes("//script");
        //
        // if (scriptNodes != null)
        // {
        //     foreach (var scriptNode in scriptNodes)
        //     {
        //         // 检查是否有 app="removable" 属性
        //         var appAttribute = scriptNode.Attributes["app"];
        //
        //         if (appAttribute != null && appAttribute.Value == "removable")
        //         {
        //             // 删除节点
        //             scriptNode.Remove();
        //         }
        //     }
        // }
        //
        // // 获取修改后的HTML内容
        // var modifiedHtml = html.DocumentNode.OuterHtml;

        var composer = new Composer(docText);
        var styleNode = composer.StyleNode;
        styleNode.BuildAllXYHF();
        return composer;
    }
}