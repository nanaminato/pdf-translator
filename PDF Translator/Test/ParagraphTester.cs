using PDF_Translate_Layout_Lib.Models.Flow;
using PDF_Translate_Layout_Lib.Utils.Load;

namespace PDF_Translator.Test;

public class ParagraphTester
{
    public static async Task Main()
    {
        // var htmlLayoutPath = @"C:\Users\betha\Desktop\html\4_PDFsam_issue65_en.json";
        var htmlLayoutPath = @"C:\Users\betha\Desktop\html\4_PDFsam_issue65_en.json";
        var htmlWorkspace = await HtmlLayoutLoader.LoadWorkSpaceFromFile(htmlLayoutPath);
        var pdfLayoutPath = @"C:\Users\betha\Desktop\html\4.json";
        var pdfWorkspace = await PdfLayoutLoader.LoadWorkSpaceFromFile(pdfLayoutPath);
        var pageFlows = FlowUtils.GetFlows(htmlWorkspace, pdfWorkspace);
        foreach (var pageFlow in pageFlows)
        {
            Console.WriteLine("Page ");
            foreach (var flow in pageFlow)
            {
                Console.WriteLine(flow);
            }
        }
        // if (htmlWorkspace?.Pages != null)
        //     foreach (var page in htmlWorkspace.Pages)
        //     {
        //         Console.WriteLine(page);
        //     }
    }
}