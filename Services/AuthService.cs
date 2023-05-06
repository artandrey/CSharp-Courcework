using System.Security.Claims;
using DB.Models;
using Exceptions;
using Microsoft.AspNetCore.Components.Authorization;

namespace Services;

public class AuthService
{
    private readonly UserService _userService;
    private readonly PasswordService _passwordService;

    private readonly CustomAuthenticationStateProvider _authenticationStateProvider;

    public AuthService(UserService userService, PasswordService passwordService, AuthenticationStateProvider authenticationStateProvider)
    {
        _userService = userService;
        _passwordService = passwordService;
        _authenticationStateProvider = (CustomAuthenticationStateProvider)authenticationStateProvider;
    }

    public async Task Register(User user)
    {
        var candidate = await _userService.GetByEmail(user.Email);
        if (candidate != null) throw new ServiceException("This user is already registered");

        var hashResult = _passwordService.HashPassword(user.Password);

        await _userService.CreateAsync(new User
        {
            Email = user.Email,
            Name = user.Name,
            Password = hashResult.Value,
            Role = user.Role,
            Salt = hashResult.Salt
        });
    }

    public async Task<User> Login(string email, string password)
    {
        var candidate = await _userService.GetByEmail(email);
        if (candidate == null) throw new ServiceException("Email or password are invalid");

        bool confiramationResult = _passwordService.VerifyPassword(password, candidate.Salt, candidate.Password);

        if (!confiramationResult) throw new ServiceException("Email or password are invalid");

        await AddUserModelToAuthState(candidate);

        return candidate;
    }

    public async Task Logout()
    {
        await _authenticationStateProvider.UpdateAuthenticationState(null);
    }

    public async Task<User> GetUser()
    {
        var result = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var stateUser = result.User;
        string? userId = stateUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) throw new ServiceException("User id not found");

        var user = await _userService.GetAsync(userId);

        if (user == null) throw new ServiceException("User not found in db");

        return user;
    }

    private async Task AddUserModelToAuthState(User userModel)
    {
        await _authenticationStateProvider.UpdateAuthenticationState(new Authentication.UserSession
        {
            UserName = userModel.Name,
            Role = userModel.Role,
            Id = userModel.Id!
        });
    }
}