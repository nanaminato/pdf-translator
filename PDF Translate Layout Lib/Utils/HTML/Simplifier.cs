using System.Text;
using System.Text.RegularExpressions;

namespace PDF_Translate_Layout_Lib.Utils.HTML;

public abstract class Simplifier
{
    public static SimplifyResult Simplify(string htmlContent)
    {
        var result = new SimplifyResult();
        var simplifyDictionary = new Dictionary<string, string>();
        const string base64Pattern = @"(data:?:[^;]+;base64,[^''"")]+)";// 最后不匹配'")
        var matches = Regex.Matches(htmlContent, base64Pattern);

        var index = 1;
        var htmlBuilder = new StringBuilder(htmlContent);

        foreach (Match match in matches)
        {
            var base64Data = match.Groups[0].Value;
            var placeholder = $"[BASE64_PLACEHOLDER_"+index+']';
            index++;
            // 将base64数据存储到字典中
            simplifyDictionary.Add(placeholder, base64Data);

            // 使用占位符替代base64数据
            htmlBuilder.Replace(base64Data, placeholder);
        }

        htmlContent = htmlBuilder.ToString();

        result.Text = htmlContent;
        result.SimplifyDictionary = simplifyDictionary;

        return result;
    }

    // 反向填充
    public static string Combination(string html, Dictionary<string, string> dictionary)
    {
        var builder = new StringBuilder(html);
        foreach (var (key, value) in dictionary)
        {
            // 将占位符替换回base64数据
            builder = builder.Replace(key, $"{value}");
        }

        return builder.ToString();
    }

}