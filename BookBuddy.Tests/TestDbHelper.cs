using BookBuddy.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BookBuddy.Tests
{
    public static class TestDbHelper
    {
        public static AppDbContext CreateSQLiteDb()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;

            var db = new AppDbContext(options);
            db.Database.EnsureCreated();

            return db;
        }
    }
}
