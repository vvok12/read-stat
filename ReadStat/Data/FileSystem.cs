using System;
using System.IO;

namespace ReadStat.Data;

public static class FileSystem
{
    public const string ImageFolder = "Images";

    public static void Initialize()
    {
        var path = Path.Combine(AppContext.BaseDirectory, ImageFolder);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);        
        }
    }
}