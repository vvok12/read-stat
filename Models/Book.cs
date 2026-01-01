using System;

namespace ReadStat.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int PagesTotal { get; set; }
    public int PagesRead { get; set; }
    public string? ImagePath { get; set; }
    public bool Completed { get; set; }
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
