using MongoDB.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DB.Models;


public class UserService
{
    public async Task<User?> GetByEmail(string email)
    {

        return await MongoDB.Entities.DB.Find<User>()
                      .Match(u => u.Email == email)
                      .ExecuteSingleAsync();
    }

    public async Task<List<User>> GetAsync()
    {
        return await MongoDB.Entities.DB.Find<User>().ExecuteAsync();
    }

    public async Task<User> GetFullAsync(string id)
    {
        return await MongoDB.Entities.DB.Find<User>().Match(u => u.ID == id).ExecuteSingleAsync();
    }

    public async Task<User> GetAsync(string id)
    {
        return await MongoDB.Entities.DB.Find<User>().Match(u => u.ID == id).Project(user => user.Exclude("Folders")).ExecuteSingleAsync();
    }

    public async Task<List<User>> GetByIdsAsync(IEnumerable<string> ids)
    {
        return await MongoDB.Entities.DB.Find<User>().Match(u => ids.Contains(u.ID)).ExecuteAsync();
    }

    public async Task CreateAsync(User newUser)
    {
        await MongoDB.Entities.DB.SaveAsync(newUser);
    }

    public async Task UpdateAsync(string id, User updatedUser)
    {

        await MongoDB.Entities.DB.Update<User>().MatchID(id)
        .ModifyWith(updatedUser)
        .ExecuteAsync();
    }

    public async Task RemoveAsync(string id)
    {
        await MongoDB.Entities.DB.DeleteAsync<User>(id);
    }
}
