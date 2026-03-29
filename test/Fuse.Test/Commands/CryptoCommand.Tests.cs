using Fuse.Commands;
using System.Security.Cryptography;

namespace Fuse.Test.Commands;

[TestClass]
public class CryptoCommandTests
{
    private readonly CryptoCommand _cryptoCommand;

    public CryptoCommandTests()
    {
        _cryptoCommand = new CryptoCommand();
    }

    [TestMethod]
    public void Hash_ValidFile_ShouldGenerateHash()
    {
        // Arrange
        string filePath = Path.Combine(Path.GetTempPath(), GenerateRandomFileName());
        File.WriteAllText(filePath, "Test content");

        // Act & Assert
        _cryptoCommand.Hash(filePath);

        // Clean up
        File.Delete(filePath);
    }

    [TestMethod]
    public void Verify_ValidFile_ShouldReturnValid()
    {
        // Arrange
        string filePath = Path.Combine(Path.GetTempPath(), GenerateRandomFileName());
        File.WriteAllText(filePath, "Test content");

        byte[] hashBytes = CalculateSHA256Hash(File.ReadAllBytes(filePath));
        string expectedHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

        // Act & Assert
        _cryptoCommand.Verify(filePath, expectedHash);

        // Clean up
        File.Delete(filePath);
    }

    [TestMethod]
    public void Verify_InvalidHash_ShouldReturnInvalid()
    {
        // Arrange
        string filePath = Path.Combine(Path.GetTempPath(), GenerateRandomFileName());
        File.WriteAllText(filePath, "Test content");

        string expectedHash = "0000000000000000000000000000000000000000000000000000000000000000";

        // Act & Assert
        _cryptoCommand.Verify(filePath, expectedHash);

        // Clean up
        File.Delete(filePath);
    }

    [TestMethod]
    public void CalculateHash_FileDoesNotExist_ShouldThrowFileNotFoundException()
    {
        // Act & Assert
        Assert.Throws<FileNotFoundException>(() => _cryptoCommand.Hash("nonexistentfile.txt"));
    }

    [TestMethod]
    public void Hash_SupportedAlgorithms_ShouldGenerateHash()
    {
        // Arrange
        string filePath = Path.Combine(Path.GetTempPath(), GenerateRandomFileName());
        File.WriteAllText(filePath, "Test content");

        string[] algorithms = new string[] { "md5", "sha1", "sha256", "sha384", "sha512" };

        foreach (var algorithm in algorithms)
        {
            // Act & Assert
            _cryptoCommand.Hash(filePath, algorithm);
        }

        // Clean up
        File.Delete(filePath);
    }

    [TestMethod]
    public void Hash_UnsupportedAlgorithm_ShouldThrowArgumentException()
    {
        // Arrange
        string filePath = Path.Combine(Path.GetTempPath(), GenerateRandomFileName());
        File.WriteAllText(filePath, "Test content");

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => _cryptoCommand.Hash(filePath, "unsupported"));

        // Clean up
        File.Delete(filePath);

        Assert.IsInstanceOfType<ArgumentException>(ex);
        Assert.Contains("Unsupported algorithm: unsupported", ex.Message);
    }

    private byte[] CalculateSHA256Hash(byte[] data)
    {
        return SHA256.HashData(data);
    }

    private static string GenerateRandomFileName()
    {
        return $"testfile_{Random.Shared.Next(1000000, 9999999)}.txt";
    }
}