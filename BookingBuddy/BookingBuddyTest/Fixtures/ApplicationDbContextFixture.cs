using BookingBuddy.Server.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BookingBuddyTest.Fixtures
{
    public class ApplicationDbContextFixture : IDisposable
    {
        public BookingBuddyServerContext DbContext { get; private set; }

        public ApplicationDbContextFixture()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<BookingBuddyServerContext>()
                .UseSqlite(connection)
                .Options;
            DbContext = new BookingBuddyServerContext(options);

            DbContext.Database.EnsureCreated();
        }

        public void Dispose() => DbContext.Dispose();
    }
}