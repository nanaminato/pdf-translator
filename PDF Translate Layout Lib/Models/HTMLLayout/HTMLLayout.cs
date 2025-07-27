namespace PDF_Translate_Layout_Lib.Models.HTMLLayout;

public class HtmlLayout
{
    /*
     * html 对应pdf的页的id
     */
    public int PageId { get; set; }
    /*
     * 该页的宽度
     */
    public double Width { get; set; }
    /*
     * 该页的高度
     */
    public double Height { get; set; }
    /*
     * 该页中子元素的详细布局
     */
    public List<Layout> Children { get; set; } = [];
    public override string ToString()
    {
        var childrenStr = Children.Count == 0 
            ? "None" 
            : string.Join(",\n", Children.Select(c => "    "+c));

        return $"HtmlLayout {{ PageId = {PageId}, Width = {Width}, Height = {Height}, Children = [\n{childrenStr}] }}";
    }
}