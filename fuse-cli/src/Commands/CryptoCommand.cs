namespace Fuse.Commands;

public class CryptoCommand
{
    /// <summary>
    /// Generate hash of a file
    /// </summary>
    /// <param name="filePath">-p, File path to calculate hash</param>
    /// <param name="algorithm">-a, Hash algorithm (md5, sha1, sha256(default), sha384, sha512)</param>
    /// <param name="base64">-b, Output in base64 format</param>
    /// <exception cref="ArgumentException"></exception>
    [Command("hash")]
    public void Hash(string filePath, string algorithm = "sha256", bool base64 = false)
    {
        var hashBytes = CalculateHash(filePath, algorithm);

        var hash = FormatHash(hashBytes, base64);

        Console.WriteLine($"{algorithm.ToUpper()} hash of {filePath}:");
        Console.WriteLine(hash);
    }

    /// <summary>
    /// Verify a file
    /// </summary>
    /// <param name="filePath">-p, File path to verify</param>
    /// <param name="expectedHash">-e, Expected hash value</param>
    /// <param name="algorithm">-a, Hash algorithm (md5, sha1, sha256(default), sha384, sha512)</param>
    /// <param name="isBase64">-b, "Hash is in base64 format"</param>
    /// <exception cref="ArgumentException"></exception>
    [Command("verify")]
    public void Verify(string filePath, string expectedHash, string algorithm = "sha256", bool isBase64 = false)
    {
        byte[] hashBytes = CalculateHash(filePath, algorithm);
        string actualHash = FormatHash(hashBytes, isBase64);

        byte[] expectedHashBytes = isBase64
            ? Convert.FromBase64String(expectedHash)
            : StringToByteArray(expectedHash);

        bool isValid = CryptographicOperations.FixedTimeEquals(hashBytes, expectedHashBytes);

        Console.WriteLine($"Verifying {filePath} with {algorithm.ToUpper()}:");
        Console.WriteLine($"Expected: {expectedHash}");
        Console.WriteLine($"Actual:   {actualHash}");
        Console.WriteLine($"Result:   {(isValid ? "VALID" : "INVALID")}");
    }

    private static byte[] CalculateHash(string filePath, string algorithm)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Cannot find {filePath}.");
        }

        using var stream = File.OpenRead(filePath);
        byte[] hashBytes = algorithm.ToLower() switch
        {
            "md5" => MD5.Create().ComputeHash(stream),
            "sha1" => SHA1.Create().ComputeHash(stream),
            "sha256" => SHA256.Create().ComputeHash(stream),
            "sha384" => SHA384.Create().ComputeHash(stream),
            "sha512" => SHA512.Create().ComputeHash(stream),
            _ => throw new ArgumentException($"Unsupported algorithm: {algorithm}")
        };

        return hashBytes;
    }

    private static string FormatHash(byte[] hashBytes, bool base64 = false)
    {
        string hash = base64
           ? Convert.ToBase64String(hashBytes)
           : Convert.ToHexStringLower(hashBytes);

        return hash;
    }

    private static byte[] StringToByteArray(string hex)
    {
        int length = hex.Length;
        byte[] bytes = new byte[length / 2];
        for (int i = 0; i < length; i += 2)
        {
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        }
        return bytes;
    }
}
