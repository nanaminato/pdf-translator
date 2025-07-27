using Newtonsoft.Json;
using PDF_Translate_Layout_Lib.Models.PDFLayout;

namespace PDF_Translate_Layout_Lib.Utils.Load;

public class PdfLayoutLoader
{
    public static async Task<PdfLayoutWorkspace?> LoadWorkSpaceFromFile(string path)
    {
        if (!File.Exists(path))
        {
            throw new Exception($"File {path} does not exist");
        }

        try
        {
            var text = await File.ReadAllTextAsync(path);
            return JsonConvert.DeserializeObject<PdfLayoutWorkspace>(text);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}