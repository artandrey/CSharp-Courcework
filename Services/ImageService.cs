using DB.Models;
using Microsoft.AspNetCore.Components.Forms;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats;

namespace Services;

public class ImagesService
{

    public static readonly long MAX_FILE_SIZE = 1024 * 1024 * 20;

    public async Task<Picture> Upload(Folder folder, IBrowserFile browserFile)
    {
        // var fullSizedImage = new Picture();

        var stream = browserFile.OpenReadStream(MAX_FILE_SIZE);


        var image = await Image.LoadAsync(stream);


        var uploadStream = new MemoryStream();
        IImageFormat format;
        image.GetConfiguration().ImageFormatsManager.TryFindFormatByMimeType(browserFile.ContentType, out format!);

        await image.SaveAsync(uploadStream, format);

        var pricture = new Picture()
        {
            Title = Path.GetFileNameWithoutExtension(browserFile.Name),
            ContentType = browserFile.ContentType,
            Width = image.Width,
            Height = image.Height,
            Folder = folder
        };

        await MongoDB.Entities.DB.SaveAsync(pricture);

        await pricture.Data.UploadAsync(uploadStream);

        return pricture;

    }

    public async Task Delete(Folder folder, Picture pictureToDelete)
    {
        await MongoDB.Entities.DB.DeleteAsync<Picture>(pictureToDelete.ID);
    }
    public async Task Delete(Folder folder, List<string> picturesToDelete)
    {
        await MongoDB.Entities.DB.DeleteAsync<Picture>(picturesToDelete);
    }


}