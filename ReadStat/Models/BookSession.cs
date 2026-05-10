using System.ComponentModel.DataAnnotations.Schema;

namespace ReadStat.Models;

public class BookSession
{
    [Column("book_session_id")]
    public int SessionId { get; set; }
    
    [Column("book_id")]
    public int BookId { get; set; }
    
    [Column("book_session_pages_read")]
    public int PagesRead { get; set; }
}