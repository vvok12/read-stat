using System;
using System.IO;
using ReadStat.Data;

namespace ReadStat.Services;

public class WarmupService
{
    public void Run()
    {
        Database.Initialize();
        FileSystem.Initialize();
    }
}