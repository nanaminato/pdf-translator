using HtmlAgilityPack;

namespace Semantic_Processor.Ratp.Easy.Select;

public class InnerClassSelector
{
    public static List<(string, string)> Select(string html)
    {
        var styleList = new List<(string, string)>();

        // 使用 HtmlAgilityPack 加载 HTML 文档
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);

        // 选择所有在 <div> 内的 <span> 标签
        var spanNodes = htmlDocument.DocumentNode.SelectNodes("//div/span");

        if (spanNodes != null)
        {
            foreach (var spanNode in spanNodes)
            {
                // 提取当前 <span> 标签的 class
                var classAttributeValue = spanNode.GetAttributeValue("class", "");

                // 提取当前 <span> 标签的 font family 和 font size
                var ff = Extractor.ExtractorFontFamily(classAttributeValue);
                var fs = Extractor.ExtractorFontSize(classAttributeValue);

                // 如果当前标签没有 font family 或 font size，则从父标签继承样式
                if (string.IsNullOrEmpty(ff))
                {
                    ff = InheritFontFamily(spanNode);
                }

                if (string.IsNullOrEmpty(fs))
                {
                    fs = InheritFontSize(spanNode);
                }

                // 将提取的样式添加到列表
                styleList.Add((ff, fs));
            }

            // 输出结果
            // foreach (var style in styleList)
            // {
            //     Console.WriteLine($"Font Family: {style.Item1}, Font Size: {style.Item2}");
            // }
        }

        return styleList;
    }

    private static string InheritFontFamily(HtmlNode node)
    {
        // 从父标签继承 font family，如果没有找到则返回空字符串
        while (node.ParentNode != null)
        {
            node = node.ParentNode;

            var classAttributeValue = node.GetAttributeValue("class", "");
            var ff = Extractor.ExtractorFontFamily(classAttributeValue);

            if (!string.IsNullOrEmpty(ff))
            {
                return ff;
            }
        }

        return "";
    }

    private static string InheritFontSize(HtmlNode node)
    {
        // 从父标签继承 font size，如果没有找到则返回空字符串
        while (node.ParentNode != null)
        {
            node = node.ParentNode;

            var classAttributeValue = node.GetAttributeValue("class", "");
            var fs = Extractor.ExtractorFontSize(classAttributeValue);

            if (!string.IsNullOrEmpty(fs))
            {
                return fs;
            }
        }

        return "";
    }
}