using CustomAttributes;
using DB.Models;
using Microsoft.AspNetCore.Mvc;
using Services;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;

namespace Controllers;

[Controller]
[Route("api/[controller]")]
public class ImagesController : Controller
{
    private readonly UserService _userService;
    private readonly FolderService _folderService;


    public ImagesController(UserService userService, FolderService folderService)
    {
        _userService = userService;
        _folderService = folderService;
    }

    [HttpGet("{folderId}/image/{imageId}")]
    [ValidateTokenAttribute]
    [ValidateFolderAccess]
    public async Task<IActionResult> GetImage(string folderId, string imageId, int? size = null, int compression = 75, string format = "webp")
    {

        var user = (User)HttpContext.Items["user"]!;
        var folder = (Folder)HttpContext.Items["folder"]!;

        var image = folder.Immages.First(image => image.ID == imageId);




        var memoryStream = new MemoryStream();


        await image.Data.DownloadAsync(memoryStream);
        memoryStream.Position = 0;
        var imageToProcess = Image.Load(memoryStream);
        if (size != null)
        {
            imageToProcess.Mutate(i => i.Resize(new ResizeOptions
            {
                Size = new Size((int)size, 0),
                Mode = ResizeMode.Max
            }));
        }

        var formatManager = new ImageFormatManager();


        var encoder = GetEncoderFromFormatString(format, compression);
        if (encoder == null) return BadRequest();

        memoryStream.SetLength(0);
        memoryStream.Position = 0;
        await imageToProcess.SaveAsync(memoryStream, encoder);
        memoryStream.Position = 0;

        // var bytes = memoryStream.ToArray();
        // System.Console.WriteLine(bytes.Length);
        return File(memoryStream, $"image/{format}");
        // Do something with the byte array...

        // var result = new FileStreamResult(stream, image.ContentType);

        // var image1 = Image.Load(stream);

        // System.Console.WriteLine(image);
        // Response.ContentLength = image.FileSize;




    }

    private IImageEncoder? GetEncoderFromFormatString(string format, int compression)
    {
        IImageEncoder encoder;
        switch (format)
        {
            case "webp":
                encoder = new WebpEncoder
                {
                    Quality = compression
                };
                break;
            case "png":
                encoder = new PngEncoder
                {

                };
                break;
            case "jpg":
            case "jpeg":
                encoder = new JpegEncoder
                {
                    Quality = compression
                };
                break;
            case "bmp":
                encoder = new BmpEncoder
                {

                };
                break;
            default:
                return null;
        }
        return encoder;

    }
}