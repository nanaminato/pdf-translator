using HtmlAgilityPack;

namespace PDF_Translate_Layout_Lib.Natp.Appender;


// 检索html的标签，并添加新的style标签

public abstract class StyleNodeAppender
{
    public static HtmlDocument Append(HtmlDocument doc, string style)
    {
        // 创建新的 <style> 元素
        var newStyleNode = HtmlNode.CreateNode($"<style>{style}</style>");

        // 查找最后一个 <style> 元素
        var lastStyleNode = doc.DocumentNode.Descendants("style").LastOrDefault();

        // 检查是否找到了要插入的位置节点
        if (lastStyleNode != null)
        {
            // 在找到的位置节点后插入新的样式节点
            lastStyleNode.ParentNode.InsertAfter(newStyleNode, lastStyleNode);
        }
        else
        {
            // 如果没有找到任何 <style> 元素，则将新的 <style> 元素插入到文档的末尾
            throw new NotSupportedException();
        }

        return doc;
    }
    public static string Append(string html, string style)
    {
        // 加载 HTML 文档
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        // 创建新的 <style> 元素
        var newStyleNode = HtmlNode.CreateNode($"<style>{style}</style>");

        // 查找最后一个 <style> 元素
        var lastStyleNode = doc.DocumentNode.Descendants("style").LastOrDefault();

        // 检查是否找到了要插入的位置节点
        if (lastStyleNode != null)
        {
            // 在找到的位置节点后插入新的样式节点
            lastStyleNode.ParentNode.InsertAfter(newStyleNode, lastStyleNode);
        }
        else
        {
            // 如果没有找到任何 <style> 元素，则将新的 <style> 元素插入到文档的末尾
            throw new NotSupportedException();
        }

        // 获取修改后的 HTML
        var modifiedHtml = doc.DocumentNode.OuterHtml;
        return modifiedHtml;
    }
}
