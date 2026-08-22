# Fuse

Fuse 是一个基于 .NET 的小工具集合，主要用于文件哈希计算和验证。它提供了简单易用的命令行接口，帮助用户快速计算文件的哈希值以及验证文件完整性。

## 功能特性

- 计算文件的多种哈希值（MD5、SHA1、SHA256、SHA384、SHA512）
- 验证文件是否与指定哈希值匹配
- 支持十六进制和 Base64 格式的哈希输出
- 基于命令行的简单操作方式

## 技术栈

- **开发语言**: C# 14.0+
- **运行时环境**: .NET 10.0+
- **框架**: ConsoleAppFramework
- **加密算法**: System.Security.Cryptography
- **测试框架**: MSTest

## 安装与使用

### 克隆项目

```bash
git clone https://github.com/trickyrat/fuse.git
cd fuse
```

### 运行项目

```bash
dotnet run --project src/Fuse.csproj [commands] [options]
```

## 命令

### hash - 计算文件哈希值

计算指定文件的哈希值。

```bash
dotnet run -- hash -p <文件路径> -a <算法> -b
```

参数：
- `-p` 或 `--filePath`: 要计算哈希的文件路径
- `-a` 或 `--algorithm`: 哈希算法 (可选，默认为 sha256)
- `-b` 或 `--base64`: 输出为 Base64 格式 (可选)

示例：
```bash
# 使用默认的 SHA256 算法
dotnet run -- hash -p ./example.txt

# 使用 MD5 算法
dotnet run -- hash -p ./example.txt -a md5

# 输出为 Base64 格式
dotnet run -- hash -p ./example.txt -a sha256 -b
```

### verify - 验证文件哈希值

验证指定文件是否与预期哈希值匹配。

```bash
dotnet run -- verify -p <文件路径> -e <预期哈希值> -a <算法> -b
```

参数：
- `-p` 或 `--filePath`: 要验证的文件路径
- `-e` 或 `--expectedHash`: 预期的哈希值
- `-a` 或 `--algorithm`: 哈希算法 (可选，默认为 sha256)
- `-b` 或 `--isBase64`: 哈希值是 Base64 格式 (可选)

示例：
```bash
# 验证文件是否与指定哈希匹配
dotnet run -- verify -p ./example.txt -e abcdef123456...

# 验证 Base64 格式的哈希
dotnet run -- verify -p ./example.txt -e someBase64String -b
```

## 测试

运行单元测试：

```bash
dotnet test
```

## 许可证

[MIT](./LICENSE.txt)