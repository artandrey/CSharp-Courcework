namespace Util;

public static class MemoryUnitConverter
{
    public static string FormatBytes(long bytes)
    {
        const int scale = 1024;
        string[] units = { "B", "KB", "MB", "GB" };
        int unitIndex = 0;
        double bytesAsDouble = bytes;

        while (bytesAsDouble >= scale && unitIndex < units.Length - 1)
        {
            bytesAsDouble /= scale;
            unitIndex++;
        }

        return $"{bytesAsDouble:0.##} {units[unitIndex]}";
    }
}
