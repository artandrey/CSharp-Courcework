using Microsoft.Extensions.Options;
using MongoDB.Driver;
using DB.Models;

namespace Services;


public class UserService : DBService<User>
{

    public UserService(IOptions<DbSettings> databaseSettings) : base(databaseSettings, "users") { }

    public async Task<User> GetByEmail(string email)
    {
        return await _collection.Find(x => x.Email == email).FirstOrDefaultAsync();
    }

    public async Task<List<User>> GetAsync() =>
        await _collection.Find(_ => true).ToListAsync();

    public async Task<User?> GetAsync(string id) =>
        await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(User newUser) =>
        await _collection.InsertOneAsync(newUser);

    public async Task UpdateAsync(string id, User updatedUser) =>
        await _collection.ReplaceOneAsync(x => x.Id == id, updatedUser);

    public async Task RemoveAsync(string id) =>
        await _collection.DeleteOneAsync(x => x.Id == id);
}