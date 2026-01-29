using System;
using System.IO;
using Avalonia.Media.Imaging;

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
    
    public static Bitmap? LoadBookCover(string? coverId)
    {
        if (string.IsNullOrEmpty(coverId))
        {
            return null;
        }

        var path = Path.Combine(AppContext.BaseDirectory, FileSystem.ImageFolder, $"{coverId}.bmp");
        if (!File.Exists(path))
        {
            return null;
        }
        
        var memoryStream = new MemoryStream(File.ReadAllBytes(path));
        return new Bitmap(memoryStream);
    }
}