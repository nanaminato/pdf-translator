namespace PDF_Translate_Layout_Lib.Utils.HTML;

/*
 * 用于减小处理文本时的额外数据量
 */
public class SimplifyResult
{
    /*
     * 简化后的文本，不包含嵌入式base64资源
     */
    public string? Text
    {
        get;
        set;
    }
    /*
     * base64资源的字典
     */
    public Dictionary<string, string>? SimplifyDictionary
    {
        get;
        set;
    }
}