using PDF_Translate_Layout_Lib.Utils.Load;

namespace PDF_Translator.Test;

public class ParagraphTester
{
    public static async Task Main()
    {
        // var htmlLayoutPath = @"C:\Users\betha\Desktop\html\4_PDFsam_issue65_en.json";
        var htmlLayoutPath = @"C:\Users\betha\Desktop\html\Easy_Setting_Box_2024-00_UM_WW_S-CHI_3.0.4_240502.0.json";
        var htmlWorkspace = await HtmlLayoutLoader.LoadWorkSpaceFromFile(htmlLayoutPath);
        if (htmlWorkspace?.Pages != null)
            foreach (var page in htmlWorkspace.Pages)
            {
                Console.WriteLine(page);
            }
    }
}