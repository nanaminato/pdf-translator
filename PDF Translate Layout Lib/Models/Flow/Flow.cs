using PDF_Translate_Layout_Lib.Models.HTMLLayout;

namespace PDF_Translate_Layout_Lib.Models.Flow;

/*
 * 一个页面中的多个流，来源于用于对pdf的划分
 */
public class Flow
{
    /*
     * 流的pageId
     */
    public int PageId
    {
        get;
        set;
    }
    /*
     * 流的id(在不同page可以接续也可以重新开始）
     */
    public int FlowId
    {
        get;
        set;
    }
    /*
     * 来自html的基本的划分在该流内部的元素位置
     */
    public List<Layout> FlowElements
    {
        get;
        set;
    } = [];
    /*
     * 来自于pdf划分的流的宽度
     */
    public double Width
    {
        get;
        set;
    }
    /*
     * 来自于pdf划分的流的高度
     */
    public double Height
    {
        get;
        set;
    }
    public override string ToString()
    {
        var flowElementsStr = string.Join(", \n", FlowElements);

        return $"Flow {{ PageId = {PageId}, FlowId = {FlowId}, FlowElements = [\n{flowElementsStr}\n], Width = {Width}, Height = {Height} }}";
    }

}