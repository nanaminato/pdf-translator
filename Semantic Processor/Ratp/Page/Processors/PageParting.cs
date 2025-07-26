using HtmlAgilityPack;
using ProcessLib.Ratp.Page.Models;

namespace Semantic_Processor.Ratp.Page.Processors;

public abstract class PageParting
{
    public static List<HtmlPage> GetPages(HtmlDocument doc,bool outer=true)
    {
        var pages = new List<HtmlPage>();

        var divs = doc.DocumentNode.SelectNodes("//div[starts-with(@id, 'pf')]");

        if (divs == null) return pages;
        var id = 1;
        foreach (var div in divs)
        {
            var page = new HtmlPage
            {
                PageId = id,
            };
            id++;

            div.Attributes.Add("rtpid", $"ratp_{page.PageId}");
            // 获取div内部信息并存储在Document属性中
            if (outer)
            {
                page.Document = div.OuterHtml;
            }
            else
            {
                page.Document = div.InnerHtml;
            }

            // 将Page对象添加到列表中
            pages.Add(page);
        }

        return pages;
    }
}