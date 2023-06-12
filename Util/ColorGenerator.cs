namespace Util;

public static class ColorGenerator
{
    public static string GenerateRandomHexColor()
    {
        Random random = new Random();

        // Generate random RGB values
        int red = random.Next(256);
        int green = random.Next(256);
        int blue = random.Next(256);

        // Convert RGB values to hexadecimal representation
        string hexColor = $"#{red:X2}{green:X2}{blue:X2}";

        return hexColor;
    }
}