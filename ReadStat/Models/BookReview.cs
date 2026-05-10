using System.ComponentModel.DataAnnotations.Schema;

namespace ReadStat.Models;

public class BookReview
{
    [Column("book_review_id")]
    public int ReviewId { get; set; }
    
    [Column("book_id")]
    public int BookId { get; set; }
    
    [Column("book_review_rating")]
    public int Rating { get; set; }
}