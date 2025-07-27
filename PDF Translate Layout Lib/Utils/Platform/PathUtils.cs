namespace PDF_Translate_Layout_Lib.Utils.Platform;

public static class PathUtils
{
    public static string ReFormatFilePath(string path)
    {
        var c = System.IO.Path.DirectorySeparatorChar;
        const char a = '\\';
        const char b = '/';
        return path.Replace(a == c ? b : a, c);
    }
}