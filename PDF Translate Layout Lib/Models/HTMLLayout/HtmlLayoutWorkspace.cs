namespace PDF_Translate_Layout_Lib.Models.HTMLLayout;

public class HtmlLayoutWorkspace
{
    public List<HtmlLayout> Pages { get; set; } = [];
    public HtmlLayout GetPage(int index)
    {
        return Pages[index];
    }
}