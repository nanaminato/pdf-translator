using PDF_Translate_Layout_Lib.Natp.Appender;

namespace PDF_Translator.Test;

public class LineIdAppenderTester
{
    public static void Main()
    {
        var htmlPath = @"C:\Users\betha\Desktop\html\4_PDFsam_issue65_en.html";
        var htmlWithIdPath = @"C:\Users\betha\Desktop\html\4_PDF_Id.html";
        var text =  File.ReadAllText(htmlPath);
        var res = LineIdAppender.Append(text);
        File.WriteAllText(htmlWithIdPath, res);
    }
}