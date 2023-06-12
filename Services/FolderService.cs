using DB.Models;
using Exceptions;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Entities;
using Services;

public class FolderService
{
    private readonly UserService _userService;
    private readonly ImagesService _imagesService;

    public FolderService(UserService userService, ImagesService imagesService)
    {
        _userService = userService;
        _imagesService = imagesService;
    }

    public async Task<Folder?> GetById(string id)
    {
        return await MongoDB.Entities.DB.Find<Folder>().Match(u => u.ID == id).ExecuteSingleAsync();
    }


    public List<Folder> GetAllByUser(User user)
    {
        return user.Folders.ToList();
    }


    public async Task<Folder> Create(User creator, Folder folder)
    {
        folder.Creator = creator.ToReference();
        await MongoDB.Entities.DB.SaveAsync(folder);
        await folder.AllowedUsers.AddAsync(creator);
        return folder;
    }

    public async Task<User> AddUserByEmail(User creator, Folder folder, string email)
    {
        if (folder.Creator.ID != creator.ID) throw new ServiceException("You are not a folder creator");
        var userToAdd = await _userService.GetByEmail(email);
        if (userToAdd == null) throw new ServiceException("This user was not found");

        await folder.AllowedUsers.AddAsync(userToAdd);

        await MongoDB.Entities.DB.Update<Folder>().MatchID(folder.ID).ModifyWith(folder).ExecuteAsync();
        return userToAdd;
    }

    // public async Task<List<User>> GetUsersForFolder(User user, Folder folder)
    // {
    //     if (!folder.CheckUserAccess(user)) throw new ServiceException("You are not a folder member");
    //     var users = await _userService.GetByIdsAsync(folder.AllowedUsers);
    //     return users;
    // }

    public async Task<Folder> RenameFolder(User creator, Folder folder, string name)
    {
        if (folder.Creator.ID != creator.ID) throw new ServiceException("You are not a folder creator");
        folder.Name = name;
        await MongoDB.Entities.DB.Update<Folder>().Match(f => f.ID == folder.ID).Modify(folder => folder.Name, name).ExecuteAsync();
        return folder;
    }

    public async Task LeaveFolder(User user, Folder folder)
    {
        if (user.ID == folder.Creator.ID) throw new ServiceException("You cannot leave from your personal folder");
        await folder.AllowedUsers.RemoveAsync(user);
    }


    public async Task Delete(string id)
    {
        var folderToDelete = await GetById(id);
        if (folderToDelete == null) throw new ServiceException("Folder not found");
        var idsToDelete = folderToDelete.Immages.Select(i => i.ID).ToList();
        await folderToDelete.Immages.DeleteAllAsync();
        await MongoDB.Entities.DB.DeleteAsync<Picture>(idsToDelete);

        await MongoDB.Entities.DB.DeleteAsync<Folder>(id);

    }

    public async Task<Folder> AddPicture(User user, Folder folder, IBrowserFile browserFile)
    {
        if (!folder.CheckUserAccess(user)) throw new ServiceException("No access");
        var picture = await _imagesService.Upload(folder, browserFile);
        await folder.Immages.AddAsync(picture);
        return folder;
    }

    public async Task<Folder> RemovePicture(User user, Folder folder, Picture picture)
    {
        if (!folder.CheckUserAccess(user)) throw new ServiceException("No access");
        await folder.Immages.RemoveAsync(picture);
        await _imagesService.Delete(folder, picture);
        return folder;
    }

    public async Task<Folder> RemovePictures(User user, Folder folder, List<string> pictures)
    {
        if (!folder.CheckUserAccess(user)) throw new ServiceException("No access");
        await folder.Immages.RemoveAsync(pictures);
        await _imagesService.Delete(folder, pictures);
        return folder;
    }

    public async Task<Picture> RenamePicture(User user, Folder folder, Picture picture, string title)
    {
        if (!folder.CheckUserAccess(user)) throw new ServiceException("No access");
        var pirctureToRename = folder.Immages.First(p => p.ID == picture.ID);
        pirctureToRename.Title = title;
        await pirctureToRename.SaveAsync();
        return pirctureToRename;
    }

    public async Task<User> DeleteUser(User creator, User userToDelete, Folder folder)
    {
        if (folder.Creator.ID != creator.ID) throw new ServiceException("You are not a folder creator");
        if (creator.ID == userToDelete.ID) throw new ServiceException("You cannot delete yourself");
        await folder.AllowedUsers.RemoveAsync(userToDelete);
        return userToDelete;
    }


}