using MongoDB.Entities;

namespace DB.Models;

[Collection("pictures")]
public class Picture : FileEntity
{
    public string Title { get; set; } = null!;
    public int Width { get; set; } = 0;
    public int Height { get; set; } = 0;
    public string ContentType { get; set; } = null!;

    public One<Folder> Folder { get; set; } = null!;

    public DateTime CreationDate { get; set; } = DateTime.Now;


}