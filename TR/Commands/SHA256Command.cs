namespace TR.Commands;

public class SHA256Command
{
    private string CalculateHash(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Cannot find {filePath}.");
        }

        using var sha256 = SHA256.Create();
        using var fileStream = File.OpenRead(filePath);
        var hash = sha256.ComputeHash(fileStream);
        var sb = new StringBuilder();
        // foreach (var item in hash)
        // {
        //     sb.Append(item.ToString("x2"));
        // }

        // return sb.ToString();
        return Convert.ToHexStringLower(hash);
    }

    /// <summary>
    /// Get SHA256 hash of a file
    /// </summary>
    /// <param name="filePath">-p, Path to file</param>
    [Command("")]
    public void Root(string filePath)
    {
        try
        {
            var hash = CalculateHash(filePath);
            Console.WriteLine($"Hash: {hash.ToUpper()}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred. Error: {ex.Message}");
        }

    }

    /// <summary>
    /// Check SHA256 hash of a file
    /// </summary>
    /// <param name="filePath">-p, Path to file</param>
    /// <param name="expectedHash">-e, Expected hash</param>
    [Command("checksum")]
    public void CheckSum(string filePath, string expectedHash)
    {
        try
        {
            var actualHash = CalculateHash(filePath);
            if (actualHash.Equals(expectedHash, StringComparison.OrdinalIgnoreCase))
                Console.WriteLine("Hash verified");
            else
                Console.WriteLine("Hash unverified");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred. Error: {ex.Message}");
        }

    }
}
