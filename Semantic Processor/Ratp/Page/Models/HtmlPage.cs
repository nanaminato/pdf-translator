namespace ProcessLib.Ratp.Page.Models;

public class HtmlPage
{
    //为持久话任务预留的属性
    public int PageId
    {
        get;
        set;
    }

    // dom文档，可转为虚拟dom进行处理
    public string? Document
    {
        get;
        set;
    }
}