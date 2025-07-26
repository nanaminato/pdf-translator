namespace PDF_Translator_Utils.Utils.Platform;

public static class FolderUtils
{
    public static string GetLastFolder(string path)
    {
        if (string.IsNullOrEmpty(path))
            throw new ArgumentException("Path cannot be null or empty.");

        // 移除路径末尾的斜杠（如果有）
        path = path.TrimEnd('\\', '/');

        // 使用路径分隔符将路径分割为文件夹名数组
        string[] folders = path.Split('\\', '/');

        // 返回最后一个文件夹名
        return folders[folders.Length - 1];
    }
    public static string[] FindPtmslnFiles(string folderPath)
    {
        var files = Directory.GetFiles(folderPath, "*.ptmsln", SearchOption.TopDirectoryOnly);
        return files;
    }

    // 创建当前路径文件或者文件夹之前的文件夹
    public static void CreateBeforeFolder(string fileOrFolder)
    {
        // 获取文件夹路径
        var folderPath = System.IO.Path.GetDirectoryName(fileOrFolder);

        // 如果文件夹路径为空，则直接返回
        if (string.IsNullOrEmpty(folderPath))
        {
            return;
        }

        // 确保文件夹存在，如果不存在则创建
        Directory.CreateDirectory(folderPath);
    }
}