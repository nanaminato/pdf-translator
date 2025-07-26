namespace PDF_Translate_Layout_Lib.Models.PDFLayout;

public class Page
{
    public int Index { get; set; }

    public List<Rectangle> Rects { get; set; } = new();
}