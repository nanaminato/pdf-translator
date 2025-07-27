namespace PDF_Translate_Layout_Lib.Ratp.Easy.Mark;

public class Rtp
{
    //Attr是属性，就像id，class一样
    //Tag是标签，就像div，span一样

    // 翻译标记
    public const string TranslateAttr = "raptr";

    public const string PageIdAttr = "rtpid";
    public const string PageIdPreAttr = "ratp";
    //捕获专用标签
    public const string TempTag = "rtp";
    //使用[]包裹进行翻译的标签用于回退之后去掉[]
    public const string WholeAttr = "whole";
}