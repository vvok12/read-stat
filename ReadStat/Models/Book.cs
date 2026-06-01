using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReadStat.Models;

public class Book
{
    [Column("book_id")]
    public int BookId { get; set; }
    
    [Column("book_title")]
    public string Title { get; set; } = string.Empty;
    
    [Column("book_pages_total")]
    public int PagesTotal { get; set; }
    
    [Column("book_cover_id")]
    public string? CoverId { get; set; }
    
    [Column("pages_read")]
    public int PagesRead { get; set; }
}