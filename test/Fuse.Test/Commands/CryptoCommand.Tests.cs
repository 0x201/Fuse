using System.Security.Cryptography;

using Fuse.Commands;

namespace Fuse.Test.Commands;

[TestClass]
public class CryptoCommandTests
{
    private readonly CryptoCommand _command = new();

    [TestMethod]
    public void Hash_ValidFile_ShouldReturnZero()
    {
        // Arrange
        string filePath = Path.Combine(Path.GetTempPath(), GenerateRandomFileName());
        File.WriteAllText(filePath, "Test content");

        try
        {
            // Act
            int exitCode = _command.Hash(filePath);

            // Assert
            Assert.AreEqual(0, exitCode);
        }
        finally
        {
            File.Delete(filePath);
        }
    }

    [TestMethod]
    public void Hash_FileDoesNotExist_ShouldReturnOne()
    {
        // Act
        int exitCode = _command.Hash("nonexistentfile.txt");

        // Assert
        Assert.AreEqual(1, exitCode);
    }

    [TestMethod]
    public void Hash_UnsupportedAlgorithm_ShouldReturnOne()
    {
        // Arrange
        string filePath = Path.Combine(Path.GetTempPath(), GenerateRandomFileName());
        File.WriteAllText(filePath, "Test content");

        try
        {
            // Act
            int exitCode = _command.Hash(filePath, "unsupported");

            // Assert
            Assert.AreEqual(1, exitCode);
        }
        finally
        {
            File.Delete(filePath);
        }
    }

    [TestMethod]
    public void Verify_ValidFileAndMatchingHash_ShouldReturnZero()
    {
        // Arrange
        string filePath = Path.Combine(Path.GetTempPath(), GenerateRandomFileName());
        File.WriteAllText(filePath, "Test content");
        byte[] hashBytes = SHA256.HashData(File.ReadAllBytes(filePath));
        string expectedHash = Convert.ToHexStringLower(hashBytes);

        try
        {
            // Act
            int exitCode = _command.Verify(filePath, expectedHash);

            // Assert
            Assert.AreEqual(0, exitCode);
        }
        finally
        {
            File.Delete(filePath);
        }
    }

    [TestMethod]
    public void Verify_ValidFileButMismatchedHash_ShouldReturnOne()
    {
        // Arrange
        string filePath = Path.Combine(Path.GetTempPath(), GenerateRandomFileName());
        File.WriteAllText(filePath, "Test content");
        string expectedHash = new('0', 64);

        try
        {
            // Act
            int exitCode = _command.Verify(filePath, expectedHash);

            // Assert
            Assert.AreEqual(1, exitCode);
        }
        finally
        {
            File.Delete(filePath);
        }
    }

    [TestMethod]
    public void Verify_InvalidHexHash_ShouldReturnOne()
    {
        // Arrange
        string filePath = Path.Combine(Path.GetTempPath(), GenerateRandomFileName());
        File.WriteAllText(filePath, "Test content");
        string invalidHex = "zzzz";

        try
        {
            // Act
            int exitCode = _command.Verify(filePath, invalidHex);

            // Assert
            Assert.AreEqual(1, exitCode);
        }
        finally
        {
            File.Delete(filePath);
        }
    }

    [TestMethod]
    public void Verify_OddLengthHex_ShouldReturnOne()
    {
        // Arrange
        string filePath = Path.Combine(Path.GetTempPath(), GenerateRandomFileName());
        File.WriteAllText(filePath, "Test content");
        string oddHex = "abc";

        try
        {
            // Act
            int exitCode = _command.Verify(filePath, oddHex);

            // Assert
            Assert.AreEqual(1, exitCode);
        }
        finally
        {
            File.Delete(filePath);
        }
    }

    [TestMethod]
    public void Verify_EmptyHash_ShouldReturnOne()
    {
        // Arrange
        string filePath = Path.Combine(Path.GetTempPath(), GenerateRandomFileName());
        File.WriteAllText(filePath, "Test content");

        try
        {
            // Act
            int exitCode = _command.Verify(filePath, "");

            // Assert
            Assert.AreEqual(1, exitCode);
        }
        finally
        {
            File.Delete(filePath);
        }
    }

    [TestMethod]
    public void Verify_WithBase64FlagAndValidBase64_ShouldReturnZero()
    {
        // Arrange
        string filePath = Path.Combine(Path.GetTempPath(), GenerateRandomFileName());
        File.WriteAllText(filePath, "Test content");
        byte[] hashBytes = SHA256.HashData(File.ReadAllBytes(filePath));
        string expectedBase64 = Convert.ToBase64String(hashBytes);

        try
        {
            // Act
            int exitCode = _command.Verify(filePath, expectedBase64, isBase64: true);

            // Assert
            Assert.AreEqual(0, exitCode);
        }
        finally
        {
            File.Delete(filePath);
        }
    }

    [TestMethod]
    public void Verify_WithBase64FlagButInvalidBase64_ShouldReturnOne()
    {
        // Arrange
        string filePath = Path.Combine(Path.GetTempPath(), GenerateRandomFileName());
        File.WriteAllText(filePath, "Test content");
        string invalidBase64 = "invalid!!!";

        try
        {
            // Act
            int exitCode = _command.Verify(filePath, invalidBase64, isBase64: true);

            // Assert
            Assert.AreEqual(1, exitCode);
        }
        finally
        {
            File.Delete(filePath);
        }
    }

    [TestMethod]
    public void Verify_UnsupportedAlgorithm_ShouldReturnOne()
    {
        // Arrange
        string filePath = Path.Combine(Path.GetTempPath(), GenerateRandomFileName());
        File.WriteAllText(filePath, "Test content");

        try
        {
            // Act
            int exitCode = _command.Verify(filePath, "anyhash", algorithm: "unsupported");

            // Assert
            Assert.AreEqual(1, exitCode);
        }
        finally
        {
            File.Delete(filePath);
        }
    }

    private static string GenerateRandomFileName()
    {
        return $"testfile_{Random.Shared.Next(1000000, 9999999)}.txt";
    }
}