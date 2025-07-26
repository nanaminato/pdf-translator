using HtmlAgilityPack;

namespace Semantic_Processor.Natp.Appender;

public class LineIdAppender
{
    public static string Append(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var rid = 1; // 初始属性值

        // 找到所有包含class属性中含有"pc"的div元素
        var divs = doc.DocumentNode.SelectNodes("//div[contains(@class, 'pc')]");

        if (divs == null) return doc.DocumentNode.OuterHtml;
        foreach (var div in divs)
        {
            // 找到下一级的所有div元素
            var childDivs = div.SelectNodes("div");

            if (childDivs == null) continue;
            foreach (var childDiv in childDivs)
            {
                // 为每个下一级div元素添加rid属性
                childDiv.SetAttributeValue("rid", rid.ToString());
                rid++; // 递增属性值
            }
        }

        // 返回修改后的HTML
        return doc.DocumentNode.OuterHtml;
    }
}