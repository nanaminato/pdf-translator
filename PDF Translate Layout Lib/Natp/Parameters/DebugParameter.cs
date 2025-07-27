using Newtonsoft.Json;
using PDF_Translate_Layout_Lib.Natp.Fonts;
using PDF_Translate_Layout_Lib.Ratp.Easy;

namespace PDF_Translate_Layout_Lib.Natp.Parameters;

public class DebugParameter
{
    public Composer? Composer
    {
        get;
        set;
    }

    public List<FontPair>? FunnyStartFontPairs
    {
        get;
        set;
    }
    public List<FontPair>? MainFontPairs
    {
        get;
        set;
    }

    public List<FontPair>? DontTranslateFontPairs
    {
        get;
        set;
    }

    public string ParagraphWidthAssociationFile
    {
        get;
        set;
    } = string.Empty;

    [JsonIgnore]
    public ExecutionTimeParameter? ExecutionTimeParameter
    {
        get;
        set;
    }

    public ExecutionTimeParameter Build()
    {
        if (ExecutionTimeParameter != null)
        {
            return ExecutionTimeParameter;
        }
        var etParameter = new ExecutionTimeParameter();
        TransferPair(MainFontPairs!,etParameter.MainMapping);
        TransferPair(DontTranslateFontPairs!,etParameter.DontTranslateMapping);
        TransferPair(FunnyStartFontPairs!,etParameter.MainMapping);
        ExecutionTimeParameter = etParameter;
        return etParameter;
    }

    private static void TransferPair(List<FontPair> debugPair, IDictionary<string, HashSet<string>> etMapping)
    {
        foreach (var pair in debugPair)
        {
            var fontFamily = pair.FontFamily.Trim();
            var fontSize = pair.FontSize.Trim();

            // 分割FontFamily和FontSize
            var fontFamilies = fontFamily.Split(',');
            var fontSizes = fontSize.Split(',');

            foreach (var ff in fontFamilies)
            {
                // ReSharper disable once InconsistentNaming
                var trimmedFF = ff.Trim();
                if(string.IsNullOrWhiteSpace(trimmedFF)) continue;
                foreach (var fs in fontSizes)
                {
                    // ReSharper disable once InconsistentNaming
                    var trimmedFS = fs.Trim();
                    if(string.IsNullOrWhiteSpace(trimmedFS)) continue;
                    AddToMapping(etMapping, trimmedFF, trimmedFS);
                }
            }
        }
    }

    private static void AddToMapping(IDictionary<string, HashSet<string>> mapping, string fontFamily, string fontSize)
    {
        // Console.WriteLine($"{fontFamily}: {fontSize}");
        if (!mapping.ContainsKey(fontFamily))
        {
            mapping[fontFamily] = new HashSet<string>();
        }

        mapping[fontFamily].Add(fontSize);
    }

    public static void Test()
    {
        var debug = new DebugParameter()
        {
            MainFontPairs = new List<FontPair>()
            {
                new()
                {
                    FontFamily = "ff1,ff2,ff3, ",
                    FontSize = "fs1,fs2"
                },
                new()
                {
                    FontFamily = "ff4,ff5, ",
                    FontSize = "fs1,"
                },
                new()
                {
                    FontFamily = "ff5",
                    FontSize = "*,"
                },
            }
        };
        var et = debug.Build();
        Console.WriteLine(et);

    }
}