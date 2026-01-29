using System;
using System.IO;
using ReadStat.Data;

namespace ReadStat.Services;

public class WarmupService
{
    public void Run()
    {
        var dbPath = Path.Combine(AppContext.BaseDirectory, "readstat.db");
        Database.Initialize(dbPath);
        FileSystem.Initialize();
    }
}