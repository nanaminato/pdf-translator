namespace Semantic_Processor.Ratp.Easy;

public abstract class ClassReplacer
{
    public static string FontReplace(string clstr,string font)
    {
        var old = Extractor.ExtractorFontFamily(clstr);
        if (old != null)
        {
            return clstr.Replace(old, font);
        }

        return old + " " + font;
    }
}