using System;
using System.IO;
using System.Security.Cryptography;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

using Fuse.Commands;

namespace Fuse.Benchmarks;

/// <summary>
/// 对 CryptoCommand 的 Hash 和 Verify 方法进行性能基准测试
/// </summary>
[MemoryDiagnoser]  // 记录内存分配
public class CryptoCommandBenchmark
{
    private CryptoCommand? _command;
    private string? _testFilePath;
    private string? _expectedHash;

    // 使用 Params 测试不同大小的文件
    [Params(1024, 10240, 102400)]  // 1KB, 10KB, 100KB
    public int FileSize { get; set; }

    // 测试不同的哈希算法
    [Params("md5", "sha1", "sha256", "sha512")]
    public string? Algorithm { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _command = new CryptoCommand();

        // 创建临时测试文件
        _testFilePath = Path.GetTempFileName();
        var data = new byte[FileSize];
        Random.Shared.NextBytes(data);
        File.WriteAllBytes(_testFilePath, data);

        // 预先计算正确哈希值用于 Verify 测试
        using var stream = File.OpenRead(_testFilePath);
        var hashBytes = Algorithm?.ToLower() switch
        {
            "md5" => MD5.HashData(stream),
            "sha1" => SHA1.HashData(stream),
            "sha256" => SHA256.HashData(stream),
            "sha512" => SHA512.HashData(stream),
            _ => throw new ArgumentException($"Unsupported algorithm: {Algorithm}")
        };
        _expectedHash = Convert.ToHexStringLower(hashBytes);
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        if (_testFilePath != null && File.Exists(_testFilePath))
        {
            File.Delete(_testFilePath);
        }
    }

    /// <summary>
    /// 基准测试：计算文件哈希
    /// </summary>
    [Benchmark(Baseline = true)]
    public int Hash()
    {
        return _command!.Hash(_testFilePath!, Algorithm!);
    }

    /// <summary>
    /// 基准测试：验证文件哈希（匹配）
    /// </summary>
    [Benchmark]
    public int Verify_Match()
    {
        return _command!.Verify(_testFilePath!, _expectedHash!, Algorithm!);
    }

    /// <summary>
    /// 基准测试：验证文件哈希（不匹配）
    /// </summary>
    [Benchmark]
    public int Verify_Mismatch()
    {
        // 使用一个明显错误的哈希值
        var wrongHash = new string('0', _expectedHash!.Length);
        return _command!.Verify(_testFilePath!, wrongHash, Algorithm!);
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        // 运行基准测试
        var summary = BenchmarkRunner.Run<CryptoCommandBenchmark>();
    }
}