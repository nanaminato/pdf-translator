namespace PDF_Translate_Layout_Lib.Models.PDFLayout;

public class PdfLayoutWorkspace
{
    /// <summary>
    /// pdf的简要标识
    /// </summary>
    public string? PdfIdentifier { get; set; }

    /// <summary>
    /// pdf的页数
    /// </summary>
    public int PageCount { get; set; }

    /// <summary>
    /// pdf中的特殊的宽度和高度，page.index 指向该page对应的dimension
    /// </summary>
    public List<Dimension> Dimensions { get; set; } = new();

    /// <summary>
    /// pdf 每一个page的方框
    /// </summary>
    public List<Page> Pages { get; set; } = new();
    public Page GetPage(int index)
    {
        return Pages[index];
    }

    public Dimension GetDimension(int index)
    {
        return Dimensions[index];
    }
    public Dimension GetPageDimensionByPageId(int pageId)
    {
        var index = Pages[pageId].Index;
        return Dimensions[index];
    }
}