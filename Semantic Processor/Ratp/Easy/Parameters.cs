namespace Semantic_Processor.Ratp.Easy;

[Obsolete]
public class Parameters
{
    // 是否存在页面的文字的断点，如果为true，意味着一张pdf中存在
    // 2列或多列文字段落。
    public string? DirectFont
    {
        get;
        set;
    }
    public bool UseBlockInTranslate
    {
        get;
        set;
    } = false;
    public bool? SpanBreak
    {
        get;
        set;
    }
    // 用于指定正文字体的盒子大小
    public double? BodyFontWidth
    {
        get;
        set;
    }
    // 指定正文字体的主要样式/全部样式
    public List<string>? BodyFontFamilies
    {
        get;
        set;
    } = new();
    // 指定正文字体大小的主要样式
    public List<string>? BodyFontSizes
    {
        get;
        set;
    } = new();
    // 指定不要翻译的字体家族的样式
    public List<string>? DontTranslateFamilies
    {
        get;
        set;
    } = new();
    // 行内忽略翻译，转化为行内盒子
    public List<string>? InlineIgnoreFontFamilies
    {
        get;
        set;
    } = new();
    public List<string>? InlineIgnoreFontSizes
    {
        get;
        set;
    } = new();
    // 行内参与翻译，但是添加[]确保不会进行实际的翻译，
    // 认为是一个整体
    public List<string>? InlineWholeFontFamilies
    {
        get;
        set;
    } = new();

    public List<string>? InlineWholeFontSizes
    {
        get;
        set;
    } = new();
}