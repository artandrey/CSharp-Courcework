using DB.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class DatabaseConnection
{
    public DatabaseConnection(IOptions<DbSettings> databaseSettings)
    {
        var connectionString = databaseSettings.Value.ConnectionString;

        var databaseName = databaseSettings.Value.DatabaseName;

        InitAsync(connectionString, databaseName);
    }

    private async void InitAsync(string connectionString, string databaseName)
    {
        await MongoDB.Entities.DB.InitAsync(databaseName,
        MongoClientSettings.FromConnectionString(
            connectionString));
    }
}