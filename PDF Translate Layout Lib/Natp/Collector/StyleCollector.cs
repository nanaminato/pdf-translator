using System.Text;

namespace PDF_Translate_Layout_Lib.Natp.Collector;

public class StyleCollector
{
    private Dictionary<string, Dictionary<string, string>> Styles
    {
        get;
    } = new();

    public void AddStyle(string firstKey, string secondKey, string secondValue)
    {
        var get = Styles.TryGetValue(firstKey, out var dictionary);
        if (get)
        {
            dictionary![secondKey] = secondValue;
        }
        else
        {
            Styles[firstKey] = new Dictionary<string, string>()
            {
                { secondKey , secondValue },
            };
        }
    }

    public string BuildStyleText()
    {
        var sb = new StringBuilder();
        foreach (var outerKey in Styles.Keys)
        {
            sb.AppendLine($"{outerKey} {{");
            var innerDict = Styles[outerKey];
            BuildStyleTextRecursive(innerDict, sb);
            sb.AppendLine("}");
        }
        return sb.ToString();
    }

    private void BuildStyleTextRecursive(Dictionary<string, string> dict, StringBuilder sb, int indentLevel = 1)
    {
        var indent = new string('\t', indentLevel);
        foreach (var key in dict.Keys)
        {
            var value = dict[key];
            sb.AppendLine($"{indent}{key}: {value};");
            var nestedStyle = dict[key].Trim('{', '}');
            if (dict[key].StartsWith("{") && dict[key].EndsWith("}"))
            {
                var nestedDict = ParseNestedStyle(nestedStyle);
                BuildStyleTextRecursive(nestedDict, sb, indentLevel + 1);
            }
        }
    }

    private Dictionary<string, string> ParseNestedStyle(string nestedStyle)
    {
        var dict = new Dictionary<string, string>();
        var stylePairs = nestedStyle.Split(',');
        foreach (var pair in stylePairs)
        {
            var keyValue = pair.Split(':');
            var key = keyValue[0].Trim();
            if (keyValue.Length == 2)
            {
                var value = keyValue[1].Trim();
                dict[key] = value;
            }
        }
        return dict;
    }

    public static void Anything()
    {
        var collector = new StyleCollector();
        collector.AddStyle("a","b","c");
        collector.AddStyle("a","c","d");
        collector.AddStyle("b","b","c");
        collector.AddStyle("b","c","d");
        var styleText = collector.BuildStyleText();
        Console.WriteLine(styleText);
    }
}