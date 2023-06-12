using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace DB.Models;

[Collection("folders")]
public class Folder : Entity
{


    public string Name { get; set; } = null!;

    public One<User> Creator { get; set; } = null!;

    [OwnerSide]
    public Many<User> AllowedUsers { get; set; } = null!;

    public Many<Picture> Immages { get; set; } = null!;

    public DateTime CreationDate { get; set; } = DateTime.Now;

    public Folder()
    {
        this.InitOneToMany(() => Immages);
        this.InitManyToMany(() => AllowedUsers, user => user.Folders);
    }

    public bool CheckUserAccess(User user)
    {
        if (this.Creator.ID == user.ID) return true;
        if (AllowedUsers.Any(u => u.ID == user.ID)) return true;
        return false;
    }
}