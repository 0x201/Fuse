using TR.Commands;
using System.Security.Cryptography;

namespace TR.Test.Commands;

public class CryptoCommandTests
{
    private readonly CryptoCommand _cryptoCommand;

    public CryptoCommandTests()
    {
        _cryptoCommand = new CryptoCommand();
    }

    [Fact]
    public void Hash_ValidFile_ShouldGenerateHash()
    {
        // Arrange
        string filePath = Path.Combine(Path.GetTempPath(), "testfile.txt");
        File.WriteAllText(filePath, "Test content");

        // Act & Assert
        var ex = Record.Exception(() => _cryptoCommand.Hash(filePath));
        
        // Clean up
        File.Delete(filePath);
        
        Assert.Null(ex);
    }

    [Fact]
    public void Verify_ValidFile_ShouldReturnValid()
    {
        // Arrange
        string filePath = Path.Combine(Path.GetTempPath(), "testfile.txt");
        File.WriteAllText(filePath, "Test content");
        
        byte[] hashBytes = CalculateSHA256Hash(File.ReadAllBytes(filePath));
        string expectedHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

        // Act & Assert
        var ex = Record.Exception(() => _cryptoCommand.Verify(filePath, expectedHash));
        
        // Clean up
        File.Delete(filePath);
        
        Assert.Null(ex);
    }

    [Fact]
    public void Verify_InvalidHash_ShouldReturnInvalid()
    {
        // Arrange
        string filePath = Path.Combine(Path.GetTempPath(), "testfile.txt");
        File.WriteAllText(filePath, "Test content");
        
        string expectedHash = "0000000000000000000000000000000000000000000000000000000000000000";

        // Act & Assert
        var ex = Record.Exception(() => _cryptoCommand.Verify(filePath, expectedHash));
        
        // Clean up
        File.Delete(filePath);
        
        Assert.Null(ex);
    }

    [Fact]
    public void CalculateHash_FileDoesNotExist_ShouldThrowFileNotFoundException()
    {
        // Act & Assert
        Assert.Throws<FileNotFoundException>(() => _cryptoCommand.Hash("nonexistentfile.txt"));
    }

    [Fact]
    public void Hash_SupportedAlgorithms_ShouldGenerateHash()
    {
        // Arrange
        string filePath = Path.Combine(Path.GetTempPath(), "testfile.txt");
        File.WriteAllText(filePath, "Test content");
        
        string[] algorithms = new string[] { "md5", "sha1", "sha256", "sha384", "sha512" };
        
        foreach (var algorithm in algorithms)
        {
            // Act & Assert
            var ex = Record.Exception(() => _cryptoCommand.Hash(filePath, algorithm));
            Assert.Null(ex);
        }
        
        // Clean up
        File.Delete(filePath);
    }

    [Fact]
    public void Hash_UnsupportedAlgorithm_ShouldThrowArgumentException()
    {
        // Arrange
        string filePath = Path.Combine(Path.GetTempPath(), "testfile.txt");
        File.WriteAllText(filePath, "Test content");
        
        // Act & Assert
        var ex = Record.Exception(() => _cryptoCommand.Hash(filePath, "unsupported"));
        
        // Clean up
        File.Delete(filePath);
        
        Assert.IsType<ArgumentException>(ex);
        Assert.Contains("Unsupported algorithm: unsupported", ex.Message);
    }

    private byte[] CalculateSHA256Hash(byte[] data)
    {
        return SHA256.HashData(data);
    }
}