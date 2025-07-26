using Semantic_Processor.Ratp.Easy.Mark;

namespace Semantic_Processor.Natp.Manipulator;

using HtmlAgilityPack;

public static class HtmlManipulator
{
    public static string ProcessSpliceWithRange(string html,int? from,int? to)
    {
        from ??= 0;
        to ??= int.MaxValue;
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        var divs = doc.DocumentNode.SelectNodes("//div[@rtpid]");

        if (divs != null)
        {
            foreach (var div in divs)
            {
                if (int.TryParse(div.GetAttributeValue(Rtp.PageIdAttr, "0")[(Rtp.PageIdPreAttr.Length + 1)..], out var rtpid))
                {
                    if (rtpid < from || rtpid > to)
                    {
                        // Remove the div if it doesn't meet the condition
                        div.Remove();
                    }
                }
            }
            return doc.DocumentNode.OuterHtml;
        }
        return html;
    }
}
