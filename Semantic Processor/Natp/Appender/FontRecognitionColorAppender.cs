namespace Semantic_Processor.Natp.Appender;

using System.Collections.Generic;
using HtmlAgilityPack;

internal abstract class FontRecognitionColorAppender
{
    public static HtmlDocument Append(HtmlDocument doc, Dictionary<string, string> fColorDict)
    {
        foreach (var (key, value) in fColorDict)
        {
            // 查找所有具有class属性的标签
            var nodes = doc.DocumentNode.SelectNodes("//*[@class]");

            if (nodes == null) continue;
            foreach (var node in nodes)
            {
                // 获取class属性值
                var classValue = node.GetAttributeValue("class", "");

                // 检查class属性值是否包含键key
                if (classValue.Contains(key))
                {
                    // 在class属性值后面追加value
                    node.SetAttributeValue("class", classValue + " " + value);
                }
            }
        }

        return doc;
    }
    public static string Append(string html, Dictionary<string, string> fColorDict)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        foreach (var (key, value) in fColorDict)
        {
            // 查找所有具有class属性的标签
            var nodes = doc.DocumentNode.SelectNodes("//*[@class]");

            if (nodes == null) continue;
            foreach (var node in nodes)
            {
                // 获取class属性值
                var classValue = node.GetAttributeValue("class", "");

                // 检查class属性值是否包含键key
                if (classValue.Contains(key))
                {
                    // 在class属性值后面追加value
                    node.SetAttributeValue("class", classValue + " " + value);
                }
            }
        }

        return doc.DocumentNode.OuterHtml;
    }
}
