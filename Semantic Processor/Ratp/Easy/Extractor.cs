using System.Text.RegularExpressions;

namespace Semantic_Processor.Ratp.Easy;

public partial class Extractor
{
    public static string? ExtractorFontFamily(string classStr)
    {
        const string findRegex = @"(ff[0-9a-fA-F]+)";
        var match = Regex.Match(classStr, findRegex);
        return !match.Success ? null : match.Groups[1].Value;
    }
    public static string? ExtractorFontSize(string classStr)
    {
        const string findRegex = @"(fs[0-9a-fA-F]+)";
        var match = MyRegex3().Match(classStr);
        return !match.Success ? null : match.Groups[1].Value;
    }
    public static string? ExtractorX(string classStr)
    {
        const string findRegex = @"(x[0-9a-fA-F]+)";
        var match = MyRegex2().Match(classStr);
        return !match.Success ? null : match.Groups[1].Value;
    }
    public static string? ExtractorY(string classStr)
    {
        const string findRegex = @"(y[0-9a-fA-F]+)";
        var match = MyRegex1().Match(classStr);
        return !match.Success ? null : match.Groups[1].Value;
    }
    public static string? ExtractorHeight(string classStr)
    {
        const string findRegex = @"(h[0-9a-fA-F]+)";
        var match = MyRegex().Match(classStr);
        return !match.Success ? null : match.Groups[1].Value;
    }

    [GeneratedRegex("(h[0-9a-fA-F]+)")]
    private static partial Regex MyRegex();
    [GeneratedRegex("(y[0-9a-fA-F]+)")]
    private static partial Regex MyRegex1();
    [GeneratedRegex("(x[0-9a-fA-F]+)")]
    private static partial Regex MyRegex2();
    [GeneratedRegex("(fs[0-9a-fA-F]+)")]
    private static partial Regex MyRegex3();
}