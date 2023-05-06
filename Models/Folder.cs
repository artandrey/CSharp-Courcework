using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace DB.Models;

public class Folder
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string Name { get; set; } = null!;

    public string Creator { get; set; } = null!;

    public List<string> AllowedUsers = new();

    public bool CheckUserAccess(User user)
    {
        if (this.Creator == user.Id) return true;
        if (this.AllowedUsers.Contains(user.Id)) return true;
        return false;
    }
}