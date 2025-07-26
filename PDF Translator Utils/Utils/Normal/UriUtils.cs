namespace PDF_Translator_Utils.Utils.Normal;

public static class UriUtils
{
    public static Uri GetUriWithDiskPath(string filePath)
    {
        return new Uri($"file:///{filePath}");
    }

    public static Uri GetUriWithRelativePath(string appPath, string relativePath)
    {
        return new Uri($"file:///{appPath}{System.IO.Path.PathSeparator}relativePath");
    }
}