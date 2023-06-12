namespace Validation.Models;

public class DownloadPropertiesModel
{
    public float Size { get; set; } = 1;

    public string Format { get; set; } = null!;

    public int Quality { get; set; } = 75;

}