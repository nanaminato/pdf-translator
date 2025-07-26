using HtmlAgilityPack;

namespace Semantic_Processor.Natp.Appender;

public static class ScriptAppender
{
    //添加opened使得每一个页面打开，以便于获得实际的渲染宽度，并添加script代码，使得下载一份宽度映射表
    public static string Append(string html, string script)
    {
        // Load the HTML document
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        var divs = doc.DocumentNode.SelectNodes("//div[contains(@class, 'pc')]");

        if (divs != null)
        {
            foreach (var div in divs)
            {
                // 获取div标签的class属性值
                var classValue = div.GetAttributeValue("class", "");

                // 如果class属性中不包含"opened"，则添加它
                if (!classValue.Contains("opened"))
                {
                    div.SetAttributeValue("class", classValue + " opened");
                }
            }
        }
        // Create a new script element
        var scriptElement = doc.CreateElement("script");
        scriptElement.InnerHtml = script;
        scriptElement.Attributes.Add("app", "removable");

        // Find the head element in the HTML document and append the script
        var headElement = doc.DocumentNode.SelectSingleNode("//head");
        if (headElement != null)
        {
            headElement.AppendChild(scriptElement);
        }
        else
        {
            // If there is no head element, create one and append the script
            var newHeadElement = doc.CreateElement("head");
            newHeadElement.AppendChild(scriptElement);
            doc.DocumentNode.AppendChild(newHeadElement);
        }

        return doc.DocumentNode.OuterHtml;
    }
}