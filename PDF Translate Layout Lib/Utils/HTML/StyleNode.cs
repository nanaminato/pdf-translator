using System.Text.RegularExpressions;

namespace PDF_Translate_Layout_Lib.Utils.HTML;

public class StyleNode
{
    // comment: 样式表的规则比较利于查询和构建，不会既包含样式又包含子样式定义
    private IDictionary<string, string>? _styles;
    public const string OriginHead = "style";// 最外层的样式,一般不使用这个Node进行查询样式，或者构建样式
    private List<StyleNode>? _nodes;
    public bool HasBuild;
    private string? _styleText;
    private bool _hasBuildChildrenNodes = false;

    public string? BlockHead
    {
        get;
        set;
    }

    //use by newtonsoft
    public StyleNode()
    {

    }
    public StyleNode(IDictionary<string, string> styles,string blockHead=OriginHead)
    {
        _styles = styles;
        HasBuild = true;
        BlockHead = blockHead;
    }

    public StyleNode(string styleText,string blockHead="style")
    {
        _styleText = styleText;
        HasBuild = false;
        BlockHead = blockHead;
    }

    public void Build()
    {
        if (HasBuild) return;
        HasBuild = true;
        _styles = new BlockReader().ReadBlocks(_styleText??"");
    }

    public void BuildChildrenNodes()
    {
        if (_hasBuildChildrenNodes) return;
        if(!HasBuild) Build();// 这里可以 无条件执行Build，因为Build内部对是否已经Build进行了判断
        if (_styles == null)
        {
            Console.WriteLine("ERROR OCCURRED in StyleNode-BuildChildrenNodes");
            return;
        }
        foreach (var (head, styles) in _styles)
        {
            var childNode = new StyleNode(styles, head);
            _nodes?.Add(childNode);
        }

        _hasBuildChildrenNodes = true;

    }

    public List<string> GetStyleHeads()
    {
        if(!HasBuild) Build();
        if (_styles == null) throw new Exception();
        return _styles.Keys.ToList();
    }

    public List<string> GetStyleBlocks()
    {
        if(!HasBuild) Build();
        if (_styles == null) throw new Exception();
        return _styles.Values.ToList();
    }
    public void ListAllStyleBlock()
    {
        if (!HasBuild)
        {
            Build();
        }

        if (_styles == null) throw new Exception();
        foreach (var (head,styles) in _styles)
        {
            Console.WriteLine($"head: {head}\n" +
                              $"styles: \n{styles}\n");
        }
    }

    // 为了简化项目，我们只关注我们需要的部分
    // 我们要查找的是 xx hx yx 其余都不重要
    //根据规则，我们没有进行子规则的查询，因为我们并不需要
    // public List<StyleSearchResult> FindClass(string classStr)
    // {
    //     return null;
    // }
    public EasyStyleSearchResult? FindClass(string classStr)
    {
        if(!HasBuild) Build();
        if (_styles == null) throw new Exception();
        var key = $".{classStr}";
        if (!_styles.ContainsKey(key)) return null;
        var easy = new EasyStyleSearchResult
        {
            FindClass = classStr,
            Result = _styles[key]
        };
        return easy;
    }

    public double? FontSize(string classStr)
    {
        if (!classStr.StartsWith("fs")) throw new Exception();
        var result = FindClass(classStr);
        if (result == null) return null;

        var regex = @"font-size:\s*([\d.]+)px;";// 忽略了不规则的css
        var match = Regex.Match(result?.Result ?? "", regex);

        if (double.TryParse(match.Groups[1].Value, out var parsedValue))
        {
            return parsedValue;
        }

        return null;
    }

    public double? Left(string classStr)
    {
        if (!classStr.StartsWith("x")) throw new Exception();
        var result = FindClass(classStr);
        if (result == null) return null;

        var regex = @"left:\s*([\d.]+)px;";// 忽略了不规则的css
        var match = Regex.Match(result?.Result ?? "", regex);

        if (double.TryParse(match.Groups[1].Value, out var parsedValue))
        {
            return parsedValue;
        }

        return null;
    }

    public double? Bottom(string classStr)
    {
        if (!classStr.StartsWith("y")) throw new Exception();
        var result = FindClass(classStr);
        if (result == null) return null;

        var regex = @"bottom:\s*([\d.]+)px;";// 忽略了不规则的css
        var match = Regex.Match(result?.Result ?? "", regex);

        if (double.TryParse(match.Groups[1].Value, out var parsedValue))
        {
            return parsedValue;
        }

        return null;
    }

    public double? Height(string classStr)
    {
        if (!classStr.StartsWith("h")) throw new Exception();
        var result = FindClass(classStr);
        if (result == null) return null;

        const string regex = @"height:\s*([\d.]+)px;"; // 忽略了不规则的css
        var match = Regex.Match(result?.Result ?? "", regex);

        if (double.TryParse(match.Groups[1].Value, out var parsedValue))
        {
            return parsedValue;
        }

        return null;
    }


    public Dictionary<string, double?> Lefts { get; set; } = new();
    public Dictionary<string, double?> Bottoms { get; set; } = new();
    public Dictionary<string, double?> FontSizes { get; set; } = new();
    public Dictionary<string, double?> Heights { get; set; } = new();
    public List<string> FontFamilies { get; set; } = new();

    public void BuildAllXYHF()
    {
        if(!HasBuild) Build();
        BuildX();
        BuildY();
        BuildF();
        BuildH();
        BuildFF();
    }

    private void BuildFF()
    {
        var i = 0;
        while (true)
        {
            var res = FindClass($"ff{i:x}");
            if (res == null)
            {
                break;
            }
            FontFamilies.Add(res.FindClass);
            i++;
        }
    }

    private void BuildX()
    {
        var i = 0;
        while (true)
        {
            var res = Left($"x{i:x}");
            if (res == null)
            {
                break;
            }
            Lefts.Add($"x{i:x}",res);
            i++;
        }
    }
    private void BuildY()
    {
        var i = 0;
        while (true)
        {
            var res = Bottom($"y{i:x}");
            if (res == null)
            {
                break;
            }
            Bottoms.Add($"y{i:x}",res);
            i++;
        }
    }
    private void BuildF()
    {
        var i = 0;
        while (true)
        {
            var res = FontSize($"fs{i:x}");
            if (res == null)
            {
                break;
            }
            FontSizes.Add($"fs{i:x}",res);
            i++;
        }
    }
    private void BuildH()
    {
        var i = 0;
        while (true)
        {
            var res = Height($"h{i:x}");
            if (res == null)
            {
                break;
            }
            Heights.Add($"h{i:x}",res);
            i++;
        }
    }

}