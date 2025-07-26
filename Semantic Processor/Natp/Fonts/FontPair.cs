namespace Semantic_Processor.Natp.Fonts;

public class FontPair
{
    public string FontFamily
    {
        get;
        set;
    } = string.Empty;

    public string FontSize
    {
        get;
        set;
    } = string.Empty;

    public override string ToString()
    {
        return $"fontFamily: {FontFamily}\n" +
               $"fontSize: {FontSize}\n";
    }
}