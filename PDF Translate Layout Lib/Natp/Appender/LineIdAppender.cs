using HtmlAgilityPack;

namespace PDF_Translate_Layout_Lib.Natp.Appender;

public class LineIdAppender
{
    /// <summary>
    /// 为html的每一页的每一个div元素添加rid
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    public static string Append(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        // 使用 XPath 查找 id 以 pf 开头的 div 节点
        var pages = doc.DocumentNode.SelectNodes("//div[starts-with(@id, 'pf')]");
        foreach (var page in pages)
        {
            // 找到所有包含class属性中含有"pc"的div元素
            var divs = page.SelectNodes("//div[contains(@class, 'pc')]");
            var rid = 1; // page 内的div元素id
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
        }
        
        // 返回修改后的HTML
        return doc.DocumentNode.OuterHtml;
    }
}