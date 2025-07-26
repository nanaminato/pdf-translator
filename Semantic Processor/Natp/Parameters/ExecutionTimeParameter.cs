using System.Text;

namespace Semantic_Processor.Natp.Parameters;

public class ExecutionTimeParameter
{
    // 代表支持所有字体大小
    // private const string All = "all";

    public readonly Dictionary<string, HashSet<string>>
        MainMapping = new();

    public readonly Dictionary<string, HashSet<string>>
        DontTranslateMapping = new();

    public readonly Dictionary<string, HashSet<string>>
        FunnyStartMapping = new();

    public bool IsMain(string fontFamily, string fontSize)
    {
        return Contains(MainMapping, fontFamily, fontSize);
    }
    public bool IsDontTranslate(string fontFamily, string fontSize)
    {
        return Contains(DontTranslateMapping, fontFamily, fontSize);
    }

    public bool IsFunnyStart(string fontFamily, string fontSize)
    {
        return Contains(FunnyStartMapping, fontFamily, fontSize);
    }

    private static bool Contains(IDictionary<string,HashSet<string>> dictionary,string fontFamily, string fontSize)
    {
        if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
        if (!dictionary.TryGetValue(fontFamily, out var sizeSet)) return false;
        return sizeSet.Contains("*") || sizeSet.Contains(fontSize)||fontSize=="";
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine("MainMapping:");
        foreach (var entry in MainMapping)
        {
            sb.AppendLine($"Key: {entry.Key}");
            sb.AppendLine("Values:");
            foreach (var value in entry.Value)
            {
                sb.AppendLine(value);
            }
        }

        sb.AppendLine("DontTranslateMapping:");
        foreach (var entry in DontTranslateMapping)
        {
            sb.AppendLine($"Key: {entry.Key}");
            sb.AppendLine("Values:");
            foreach (var value in entry.Value)
            {
                sb.AppendLine(value);
            }
        }
        sb.AppendLine("FunnyStartMapping:");
        foreach (var entry in FunnyStartMapping)
        {
            sb.AppendLine($"Key: {entry.Key}");
            sb.AppendLine("Values:");
            foreach (var value in entry.Value)
            {
                sb.AppendLine(value);
            }
        }

        return sb.ToString();
    }
}
