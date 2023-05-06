using DB.Models;
using Exceptions;
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
        var result = await _folderService.GetAllByUser(user);
        return result;
    }

    public async Task Delete(string id)
    {
        var folderToDelete = await _folderService.GetById(id);
        if (folderToDelete == null) throw new ServiceException("Folder not found");
        var user = await _authService.GetUser();
        if (user.Id != folderToDelete.Creator) throw new ServiceException("You are not a creator of this folder");
        await _folderService.Delete(id);
    }

    public async Task AddUserByEmail(Folder folder, string email)
    {
        var user = await _authService.GetUser();
        await _folderService.AddUserByEmail(user, folder, email);
    }
}