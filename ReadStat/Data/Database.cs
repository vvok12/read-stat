using System;
using System.Collections.Generic;
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
            CREATE TABLE IF NOT EXISTS Books (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Title TEXT NOT NULL,
                PagesTotal INTEGER,
                PagesRead INTEGER,
                CoverId TEXT,
                Completed INTEGER,
                Rating INTEGER,
                CreatedAt TEXT
            );");
    }

    public static List<Book> GetAllBooks()
    {
        using var conn = GetConnection();
        conn.Open();
        var items = conn.Query<Book>("SELECT * FROM Books ORDER BY CreatedAt DESC").AsList();
        return items;
    }

    public static async Task<List<Book>> ListCompletedBooksAsync()
    {
        await using var conn = GetConnection();
        await conn.OpenAsync();
        var items = await conn
            .QueryAsync<Book>("SELECT * FROM Books WHERE Completed=1 ORDER BY CreatedAt DESC");
        return items.AsList();
    }
    
    public static async Task<List<Book>> ListUnfinishedBooksAsync()
    {
        await using var conn = GetConnection();
        await conn.OpenAsync();
        var items = await conn
            .QueryAsync<Book>("SELECT * FROM Books WHERE Completed=0 ORDER BY CreatedAt DESC");
        return items.AsList();
    }

    public static async Task<int> CountCompletedBooksAsync()
    {
        await using var conn = GetConnection();
        await conn.OpenAsync();
        var count = await conn
            .ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Books WHERE Completed=1");
        return count;
    }

    public static async Task<int> CountPagesReadAsync()
    {
        await using var conn = GetConnection();
        await conn.OpenAsync();
        var totalPages = await conn
            .ExecuteScalarAsync<int>("SELECT sum(PagesRead) FROM Books");
        return totalPages;
    }

    public static Book? GetBook(int id)
    {
        using var conn = GetConnection();
        conn.Open();
        return conn.QuerySingleOrDefault<Book>("SELECT * FROM Books WHERE Id = @Id", new { Id = id });
    }

    public static int AddOrUpdate(Book b)
    {
        using var conn = GetConnection();
        conn.Open();
        if (b.Id == 0)
        {
            var sql = "INSERT INTO Books (Title, PagesTotal, PagesRead, CoverId, Completed, Rating, CreatedAt) VALUES (@Title,@PagesTotal,@PagesRead,@CoverId,@Completed,@Rating,@CreatedAt); SELECT last_insert_rowid();";
            var id = conn.ExecuteScalar<long>(sql, b);
            return (int)id;
        }
        else
        {
            var sql = "UPDATE Books SET Title=@Title, PagesTotal=@PagesTotal, PagesRead=@PagesRead, CoverId=@CoverId, Completed=@Completed, Rating=@Rating WHERE Id=@Id";
            conn.Execute(sql, b);
            return b.Id;
        }
    }

    public static void Delete(int id)
    {
        using var conn = GetConnection();
        conn.Open();
        conn.Execute("DELETE FROM Books WHERE Id=@Id", new { Id = id });
    }
}
