namespace Fuse.Commands;

public class CryptoCommand
{
    /// <summary>
    /// Generate hash of a file
    /// </summary>
    /// <param name="filePath">-p, File path to calculate hash</param>
    /// <param name="algorithm">-a, Hash algorithm (md5, sha1, sha256(default), sha512)</param>
    /// <param name="base64">-b, Output in base64 format</param>
    /// <exception cref="ArgumentException"></exception>
    [Command("hash")]
    public int Hash(string filePath, string algorithm = "sha256", bool base64 = false)
    {
        try
        {
            var hashBytes = CalculateHash(filePath, algorithm);

            var hash = FormatHash(hashBytes, base64);

            Console.WriteLine($"{filePath}-{algorithm.ToUpper()}:{hash}");
            return 0;
        }
        catch (Exception ex) when (ex is FileNotFoundException
            or ArgumentException
            or UnauthorizedAccessException
            or IOException)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            return 1;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Internal Error: {ex.Message}");
#if DEBUG
            Console.Error.WriteLine(ex.StackTrace);
#endif
            return 1;
        }
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
    public int Verify(string filePath, string expectedHash, string algorithm = "sha256", bool isBase64 = false)
    {
        try
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

            return isValid ? 0 : 1;
        }
        catch (Exception ex) when (ex is FileNotFoundException
            or ArgumentException
            or UnauthorizedAccessException
            or IOException)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            return 1;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Internal Error: {ex.Message}");
#if DEBUG
            Console.Error.WriteLine(ex.StackTrace);
#endif
            return 1;
        }
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
            "md5" => MD5.HashData(stream),
            "sha1" => SHA1.HashData(stream),
            "sha256" => SHA256.HashData(stream),
            "sha512" => SHA512.HashData(stream),
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
        hex = hex.Replace(" ", "").Replace("\t", "").Replace("\n", "").Replace("\r", "");
        if (string.IsNullOrEmpty(hex))
        {
            throw new ArgumentException("Hash value cannot be empty");
        }

        int length = hex.Length;
        if (length % 2 != 0)
        {
            throw new ArgumentException("Invalid hex string length, the length must be an even number");
        }
        
        byte[] bytes = new byte[length / 2];
        for (int i = 0; i < length; i += 2)
        {
            if (!byte.TryParse(hex.AsSpan(i, 2), System.Globalization.NumberStyles.HexNumber, null, out bytes[i / 2]))
            {
                throw new FormatException($"Invalid Hex string sequence: {hex.Substring(i, 2)}");
            }
        }
        return bytes;
    }
}
