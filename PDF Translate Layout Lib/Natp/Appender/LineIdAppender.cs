using HtmlAgilityPack;

namespace PDF_Translate_Layout_Lib.Natp.Appender;

public class LineIdAppender
{
    public static string Append(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var rid = 1; // 初始属性值

        // 找到所有包含class属性中含有"pc"的div元素
        var divs = doc.DocumentNode.SelectNodes("//div[contains(@class, 'pc')]");

        foreach (var div in divs)
        {
            // 找到下一级的所有div元素
            var childDivs = div.SelectNodes("div");

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