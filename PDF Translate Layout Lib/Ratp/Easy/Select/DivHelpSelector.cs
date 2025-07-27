using HtmlAgilityPack;
using PDF_Translate_Layout_Lib.Ratp.Easy.Mark;

namespace PDF_Translate_Layout_Lib.Ratp.Easy.Select;

public static class DivHelpSelector
{
    public static HtmlNode Select(string main,string selectRoot)
    {
        var rich = $"<{Rtp.TempTag}>{main}</{Rtp.TempTag}>";
        var document = new HtmlDocument();
        document.LoadHtml(rich);
        return document.DocumentNode.SelectSingleNode($"//{Rtp.TempTag}/{selectRoot}");
    }
}