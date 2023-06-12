using System.Security.Cryptography;
using System.Text;

namespace Services;

public class HashResult
{
    public string Value { get; set; } = null!;
    public string Salt { get; set; } = null!;
}

public class PasswordService
{
    public HashResult HashPassword(string rawPassword)
    {
        // Generate a random salt
        string salt = GenerateSalt();

        // Hash the raw password using the salt
        string hashedPassword = HashPassword(rawPassword, salt);

        return new HashResult
        {
            Salt = salt,
            Value = hashedPassword
        };
    }

    public bool VerifyPassword(string rawPassword, string salt, string hashedPassword)
    {
        // Hash the raw password using the stored salt
        string hashedInput = HashPassword(rawPassword, salt);

        // Compare the hashed input with the stored hashed password
        return hashedInput == hashedPassword;
    }

    private string GenerateSalt()
    {
        // Generate a random salt value using a secure cryptographic random number generator
        byte[] saltBytes = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }

        // Convert the salt bytes to a string representation
        string salt = Convert.ToBase64String(saltBytes);

        return salt;
    }

    private string HashPassword(string password, string salt)
    {
        // Concatenate the password and salt
        string saltedPassword = password + salt;

        // Hash the salted password using a secure hashing algorithm like bcrypt, Argon2, or PBKDF2
        // For the sake of simplicity, let's use a simple hash function for demonstration purposes
        string hashedPassword = SimpleHashFunction(saltedPassword);

        return hashedPassword;
    }

    private string SimpleHashFunction(string input)
    {
        // Implement your own secure hash function here
        // This is a simple demonstration and should not be used in production
        using (var md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            string hashedInput = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
            return hashedInput;
        }
    }
}