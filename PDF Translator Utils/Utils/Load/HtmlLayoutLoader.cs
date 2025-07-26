using Newtonsoft.Json;
using PDF_Translate_Layout_Lib.Models.HTMLLayout;

namespace PDF_Translator_Utils.Utils.Load;

public class HtmlLayoutLoader
{
    public static async Task<HtmlLayoutWorkspace?> LoadWorkSpaceFromFile(string path)
    {
        if (!File.Exists(path))
        {
            throw new Exception($"File {path} does not exist");
        }

        try
        {
            var text = await File.ReadAllTextAsync(path);
            var pages = JsonConvert.DeserializeObject<HtmlLayout[]>(text);
            if(pages==null) return null;
            return new HtmlLayoutWorkspace()
            {
                Pages = pages.ToList()
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}