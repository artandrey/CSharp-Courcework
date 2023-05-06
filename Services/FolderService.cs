using DB.Models;
using Exceptions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Services;

public class FolderService : DBService<Folder>
{
    private readonly UserService _userService;

    public FolderService(IOptions<DbSettings> databaseSettings, UserService userService) : base(databaseSettings, "folders")
    {
        _userService = userService;
    }

    public async Task<Folder?> GetById(string id)
    {
        return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }


    public async Task<List<Folder>> GetAllByUser(User user)
    {
        var filter = Builders<Folder>.Filter.Where(f => f.Creator == user.Id || f.AllowedUsers.Any(u => u == user.Id));
        var folders = await _collection.Find(filter).ToListAsync();
        return folders;
    }

    public async Task<Folder> Create(User creator, Folder folder)
    {
        folder.Creator = creator.Id;
        await _collection.InsertOneAsync(folder);
        return folder;
    }

    public async Task AddUserByEmail(User creator, Folder folder, string email)
    {
        if (folder.Creator != creator.Id) throw new ServiceException("You are not a folder creator");
        var userToAdd = await _userService.GetByEmail(email);
        if (userToAdd == null) throw new ServiceException("This user was not found");

        folder.AllowedUsers.Add(userToAdd.Id);

        var filter = Builders<Folder>.Filter.Eq(f => f.Id, folder.Id);
        var update = Builders<Folder>.Update.Set(f => f.AllowedUsers, folder.AllowedUsers);
        await _collection.UpdateOneAsync(filter, update);
    }


    public async Task Delete(string id) =>
        await _collection.DeleteOneAsync(x => x.Id == id);

}