using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;
using Util;

namespace DB.Models;

[Collection("users")]
public class User : Entity
{
    public string Name { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public string HexColor { get; set; } = ColorGenerator.GenerateRandomHexColor();



    [InverseSide]
    public Many<Folder> Folders { get; set; } = null!;

    public User()
    {
        this.InitManyToMany(() => Folders, folder => folder.AllowedUsers);
    }
}