using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

public class ValidateFolderAccess : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Items.TryGetValue("userId", out var userIdObj) || !(userIdObj is string userId))
        {
            context.Result = new StatusCodeResult(403);
            return;
        }

        var userService = context.HttpContext.RequestServices.GetService<UserService>();
        var folderService = context.HttpContext.RequestServices.GetService<FolderService>();

        if (userService == null || folderService == null)
        {
            context.Result = new StatusCodeResult(500);
            return;
        }

        var user = await userService.GetAsync(userId);

        if (!context.RouteData.Values.TryGetValue("folderId", out var folderIdObj) || !(folderIdObj is string folderId))
        {
            context.Result = new NotFoundResult();
            return;
        }

        var folder = await folderService.GetById(folderId);

        if (folder != null && !folder.CheckUserAccess(user))
        {
            context.Result = new NotFoundResult();
            return;
        }

        context.HttpContext.Items["user"] = user;
        context.HttpContext.Items["folder"] = folder;

        await next();
    }
}
