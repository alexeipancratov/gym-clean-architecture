using GymManagement.Infrastructure.Common.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Application.SubcutaneousTests.Common;

public class SqliteTestDatabase : IDisposable
{
    public SqliteConnection Connection { get; }
    
    public static SqliteTestDatabase CreateAndInitialize()
    {
        var testDatabase = new SqliteTestDatabase("DataSource=:memory:");
        
        testDatabase.InitializeDatabase();

        return testDatabase;
    }

    private void InitializeDatabase()
    {
        Connection.Open();
        var options = new DbContextOptionsBuilder<GymManagementDbContext>()
            .UseSqlite(Connection)
            .Options;

        var context = new GymManagementDbContext(options, null!);

        context.Database.EnsureCreated();
    }

    private SqliteTestDatabase(string datasourceMemory)
    {
        Connection = new SqliteConnection(datasourceMemory);
    }
    
    public void Dispose()
    {
        Connection.Close();
    }

    public void ResetDatabase()
    {
        Connection.Close();
        
        InitializeDatabase();
    }
}