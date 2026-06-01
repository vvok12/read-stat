using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;
using ReadStat.Models;

namespace ReadStat.Data;

public static class Database
{
    private static string DbPath => Path.Combine(AppContext.BaseDirectory, "db.sqlite");
    private static SqliteConnection GetConnection() => new SqliteConnection($"Data Source={DbPath}");
    
    public static void Initialize()
    {
        var folder = Path.GetDirectoryName(DbPath);
        if (!string.IsNullOrEmpty(folder) && !Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        using var conn = GetConnection();
        conn.Execute(@"
            CREATE TABLE IF NOT EXISTS books (
                book_id INTEGER PRIMARY KEY AUTOINCREMENT,
                book_title TEXT NOT NULL,
                book_pages_total INTEGER NOT NULL,
                book_cover_id TEXT,
                book_ts DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP 
            );

            CREATE TABLE IF NOT EXISTS book_sessions (
                book_session_id INTEGER PRIMARY KEY AUTOINCREMENT,
                book_id INTEGER NOT NULL,
                book_session_pages_read INTEGER NOT NULL,
                book_session_ts DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                FOREIGN KEY (book_id) REFERENCES books(book_id)
            );

            CREATE TABLE IF NOT EXISTS book_reviews (
                book_review_id INTEGER PRIMARY KEY AUTOINCREMENT,
                book_id INTEGER NOT NULL,
                book_review_rating INTEGER NOT NULL,
                book_review_ts DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                FOREIGN KEY (book_id) REFERENCES books(book_id)
            );

            CREATE VIEW IF NOT EXISTS completed_books AS
            SELECT * FROM books 
            WHERE book_pages_total <= 
                (SELECT sum(book_session_pages_read) FROM book_sessions WHERE book_id = books.book_id);

            CREATE VIEW IF NOT EXISTS unfinished_books AS
            SELECT b.book_id,
                   b.book_title,
                   b.book_pages_total,
                   b.book_cover_id,
                   b.book_ts,
                   coalesce(bs.pages_read, 0) as pages_read
            FROM books b LEFT JOIN (
                SELECT bs.book_id as bs_book_id, SUM(bs.book_session_pages_read) as pages_read
                FROM book_sessions bs
                GROUP BY bs.book_id
            ) bs ON b.book_id = bs.bs_book_id
            WHERE book_pages_total > pages_read or pages_read is null;

            CREATE VIEW IF NOT EXISTS month_reads AS 
            SELECT
                sum(book_session_pages_read) as PageSum,
                ceil(julianday(date(book_session_ts)) - julianday('now')) as DayBefore
            FROM book_sessions
            WHERE book_session_ts > date('now', '-30 day')
            GROUP BY date(book_session_ts)
            ORDER BY DayBefore;
");
    }

    public static async Task<List<Book>> ListCompletedBooksAsync()
    {
        await using var conn = GetConnection();
        await conn.OpenAsync();
        var items = await conn
            .QueryAsync<Book>("SELECT * FROM completed_books ORDER BY book_ts DESC");
        return items.AsList();
    }
    
    public static async Task<List<Book>> ListUnfinishedBooksAsync()
    {
        await using var conn = GetConnection();
        await conn.OpenAsync();
        var items = await conn
            .QueryAsync<Book>("SELECT * FROM unfinished_books ORDER BY book_ts DESC");
        return items.AsList();
    }

    public static async Task<int> CountCompletedBooksAsync()
    {
        await using var conn = GetConnection();
        await conn.OpenAsync();
        var count = await conn
            .ExecuteScalarAsync<int>("SELECT COUNT(*) FROM completed_books");
        return count;
    }

    public static async Task<int> CountPagesReadAsync()
    {
        await using var conn = GetConnection();
        await conn.OpenAsync();
        var totalPages = await conn
            .ExecuteScalarAsync<int>("SELECT sum(book_session_pages_read) FROM book_sessions");
        return totalPages;
    }

    public static async Task<List<DailyReads>> GetLastMonthDailyReadsAsync()
    {
        await using var conn = GetConnection();
        await conn.OpenAsync();
        var reads = await conn
            .QueryAsync<DailyReads>("SELECT * FROM month_reads");
        return reads.AsList();
    }

    public static async Task<int> AddOrUpdate(Book b)
    {
        await using var conn = GetConnection();
        conn.Open();
        var tr = await conn.BeginTransactionAsync();
        var bookId = await UpsertBookAsync(conn, tr, b);
        await UpdateProgressAsync(conn, tr, new BookProgresUpdateParams(bookId, b.PagesTotal), b.PagesRead);
        await tr.CommitAsync();
        return bookId;
    }

    record struct BookProgresUpdateParams(int bookId, int maxPages);
    private static async Task UpdateProgressAsync(SqliteConnection conn, DbTransaction tr, BookProgresUpdateParams book,
        int pagesRead)
    {
        int currentTotal = await conn.ExecuteScalarAsync<int?>(
            "SELECT sum(book_session_pages_read) FROM book_sessions WHERE book_id = @BookId",
            new 
            {
                @BookId = book.bookId,
            }, 
            transaction: tr) ?? 0;

        var readAdjusted = Math.Max(Math.Min(pagesRead, book.maxPages), 0);
        var diff = readAdjusted - currentTotal;
        if (diff == 0) { return; }

        await conn.ExecuteAsync(
            "INSERT INTO book_sessions(book_id, book_session_pages_read) VALUES (@BookId, @Diff)",
            new
            {
                @BookId = book.bookId,
                @Diff = diff
            }, tr);
    }

    private static async Task<int> UpsertBookAsync(SqliteConnection conn, DbTransaction tr, Book b)
    {
        if (b.BookId == 0)
        {
            var sql = "INSERT INTO Books (book_title, book_pages_total, book_cover_id) VALUES (@Title,@PagesTotal,@CoverId); SELECT last_insert_rowid();";
            var id = await conn.ExecuteScalarAsync<long>(sql, new
            {
                Title = b.Title,
                PagesTotal = b.PagesTotal,
                CoverId = b.CoverId
            }, tr);
            return (int)id;
        }
        else
        {
            var sql = "UPDATE Books SET book_title=@Title, book_pages_total=@PagesTotal, book_cover_id=@CoverId WHERE book_id=@Id";
            await conn.ExecuteAsync(sql, new
            {
                Title = b.Title,
                PagesTotal = b.PagesTotal,
                CoverId = b.CoverId,
                Id = b.BookId
            }, tr);
            return b.BookId;
        }
    }

    public static void Delete(int id)
    {
        using var conn = GetConnection();
        conn.Open();
        conn.Execute("DELETE FROM books WHERE book_id=@Id", new { Id = id });
    }
}
