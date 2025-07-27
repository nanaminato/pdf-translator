using System.Text;
using HtmlAgilityPack;
using PDF_Translate_Layout_Lib.Utils.HTML;

namespace PDF_Translate_Layout_Lib.Ratp.Easy;

public class Composer
{
    public bool Final = false;
    public SimplifyResult? Result
    {
        get;
        set;
    }

    public string? MainContainer;
    public string? Window;
    public string? Pluggable;
    public const string MainContentPlaceHolder = $"PLACEHOLDER_FOR_BODY";

    public Composer()
    {

    }
    public Composer(string text)
    {
        Result = Simplifier.Simplify(text);
        var doc = new HtmlDocument();
        doc.LoadHtml(Result.Text);
        var node = doc.DocumentNode.SelectSingleNode("//div[@id='page-container']");
        MainContainer = node.OuterHtml;
        node.InnerHtml = "";
        node.Id = "Removable";
        Window = doc.DocumentNode.OuterHtml;
        var builder = new StringBuilder();
        builder.Append(Window);
        builder.Replace(@"<div id=""Removable""></div>", MainContentPlaceHolder);
        Pluggable = builder.ToString();
    }

    private static string Splicing(List<string> pages)
    {
        var builder = new StringBuilder();
        builder.Append(@"<div id=""page-container"">");
        foreach (var page in pages)
        {
            builder.Append(page);
        }

        builder.Append(@"</div>");
        return builder.ToString();
    }

    public string OutHtmlOneStep(List<string> pages)
    {
        var step1 = Splicing(pages);
        return Compose(step1);
    }
    // 构建主要内容之后和主要主体结合
    private string Compose(string main)
    {
        var builder = new StringBuilder(Pluggable);
        builder.Replace(MainContentPlaceHolder, main);
        var outStr = Simplifier.Combination(builder.ToString(), Result!.SimplifyDictionary!);
        return outStr;
    }

    public StyleNode StyleNode
    {
        get
        {
            if (_node != null)
            {
                return _node;
            }

            var document = new HtmlDocument();
            document.LoadHtml(Pluggable);
            var nodes = document.DocumentNode.SelectNodes("//style");
            var builder = new StringBuilder();
            foreach (var node in nodes)
            {
                builder.Append(node.InnerHtml);
            }
            var style = builder.ToString();
            var styles = new BlockReader().ReadBlocks(style);


            _node = new StyleNode(styles);

            return _node;
        }
        set => _node = value;
    }

    private StyleNode? _node;

}