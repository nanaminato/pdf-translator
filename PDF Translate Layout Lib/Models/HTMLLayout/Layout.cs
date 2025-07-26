namespace PDF_Translate_Layout_Lib.Models.HTMLLayout;

public class Layout
{
    public int Id { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public double Left { get; set; }
    public double Top { get; set; }
    public override string ToString()
    {
        return $"Layout {{ Id = {Id}, Width = {Width}, Height = {Height}, Left = {Left}, Top = {Top} }}";
    }
}