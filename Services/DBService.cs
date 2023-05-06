using DB.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class DBService<T> where T : class
{

    protected readonly IMongoCollection<T> _collection;
    public DBService(IOptions<DbSettings> databaseSettings, string collectionName)
    {
        var mongoClient = new MongoClient(
            databaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            databaseSettings.Value.DatabaseName);

        _collection = mongoDatabase.GetCollection<T>(
            collectionName);
    }
}