using System.ComponentModel.DataAnnotations.Schema;

namespace ReadStat.Models;

public class CompletedBook: Book
{
    [Column("review")]
    public int? Rating { get; set; }
}