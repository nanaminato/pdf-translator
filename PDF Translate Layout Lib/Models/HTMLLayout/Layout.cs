namespace PDF_Translate_Layout_Lib.Models.HTMLLayout;

public class Layout
{
    /*
     * html中某一页的某一个元素的布局id
     * 
     */
    public int Id { get; set; }
    /*
     * 该元素的宽度
     */
    public double Width { get; set; }
    /*
     * 该元素的高度
     */
    public double Height { get; set; }
    /*
     * 该元素相对于该页的左边界的偏移位置
     */
    public double Left { get; set; }
    /*
     * 该元素相对于该页的上边界的偏移位置
     */
    public double Top { get; set; }
    public override string ToString()
    {
        return $"    Layout {{ Id = {Id}, Width = {Width}, Height = {Height}, Left = {Left}, Top = {Top} }}";
    }
}