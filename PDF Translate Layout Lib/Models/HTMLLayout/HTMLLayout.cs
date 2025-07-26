namespace PDF_Translate_Layout_Lib.Models.HTMLLayout;

public class HtmlLayout
{
    public int PageId { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public List<Layout> Children { get; set; } = [];
    public override string ToString()
    {
        var childrenStr = Children.Count == 0 
            ? "None" 
            : string.Join(",\n", Children.Select(c => "    "+c));

        return $"HtmlLayout {{ PageId = {PageId}, Width = {Width}, Height = {Height}, Children = [\n{childrenStr}] }}";
    }
}