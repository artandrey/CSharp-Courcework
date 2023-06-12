using DB.Models;
using Exceptions;
using Microsoft.AspNetCore.Components.Forms;
using MongoDB.Driver;

namespace Services;

public class AuthorisedFolderService
{
    private readonly FolderService _folderService;

    private readonly AuthService _authService;

    public AuthorisedFolderService(FolderService folderService, AuthService authService)
    {
        _folderService = folderService;
        _authService = authService;
    }

    public async Task<Folder> GetById(string id)
    {
        var user = await _authService.GetUser();
        var folder = await _folderService.GetById(id);
        if (folder == null) throw new ServiceException("Not found");
        if (!folder.CheckUserAccess(user)) throw new ServiceException("Not found");
        return folder;
    }

    public async Task<Folder> Create(Folder folder)
    {
        var user = await _authService.GetUser();
        var result = await _folderService.Create(user, folder);
        return result;
    }

    public async Task<List<Folder>> GetAll()
    {
        var user = await _authService.GetUser();
        var result = _folderService.GetAllByUser(user);
        return result;
    }

    public async Task Delete(string id)
    {
        var folderToDelete = await _folderService.GetById(id);
        if (folderToDelete == null) throw new ServiceException("Folder not found");
        var user = await _authService.GetUser();
        if (user.ID != folderToDelete.Creator.ID) throw new ServiceException("You are not a creator of this folder");
        await _folderService.Delete(id);
    }

    public async Task<User> AddUserByEmail(string folderId, string email)
    {
        var user = await _authService.GetUser();
        var folder = await GetById(folderId);

        return await _folderService.AddUserByEmail(user, folder, email);
    }

    public async Task<Folder> RenameFolder(string folderId, string name)
    {
        var user = await _authService.GetUser();
        var folder = await GetById(folderId);

        return await _folderService.RenameFolder(user, folder, name);
    }

    public async Task LeaveFolder(Folder folder)
    {
        var user = await _authService.GetUser();
        await _folderService.LeaveFolder(user, folder);
    }

    public async Task<Folder> AddPicture(string folderId, IBrowserFile browserFile)
    {
        var user = await _authService.GetUser();
        var folder = await GetById(folderId);

        return await _folderService.AddPicture(user, folder, browserFile);
    }

    public async Task<Folder> RemovePicture(string folderId, Picture picture)
    {
        var user = await _authService.GetUser();
        var folder = await GetById(folderId);

        return await _folderService.RemovePicture(user, folder, picture);
    }

    public async Task<Picture> RenamePicture(string folderId, Picture picture, string title)
    {
        var user = await _authService.GetUser();
        var folder = await GetById(folderId);

        return await _folderService.RenamePicture(user, folder, picture, title);
    }

    public async Task<User> DeleteUser(User userToDelete, string folderId)
    {
        var user = await _authService.GetUser();
        var folder = await GetById(folderId);

        return await _folderService.DeleteUser(user, userToDelete, folder);
    }

    public async Task<Folder> DeletePictures(string folderId, List<string> pictures)
    {
        var user = await _authService.GetUser();
        var folder = await GetById(folderId);

        return await _folderService.RemovePictures(user, folder, pictures);
    }

    // public async Task<List<User>> GetUsersForFolder(Folder folder)
    // {
    //     var user = await _authService.GetUser();
    //     return await _folderService.GetUsersForFolder(user, folder);
    // }
}