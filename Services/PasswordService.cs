namespace Services;

public class HashResult
{
    public string Value { get; set; } = null!;
    public string Salt { get; set; } = null!;
}

public class PasswordService
{
    public HashResult HashPassword(string rawPassowrd)
    {
        return new HashResult
        {
            Salt = "123",
            Value = rawPassowrd
        };
    }

    public bool VerifyPassword(string rawPassowrd, string salt, string hashedPassword)
    {
        return rawPassowrd == hashedPassword;
    }
}