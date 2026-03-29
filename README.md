# Fuse

Fuse is a collection of small utilities built on .NET, primarily designed for file hash calculation and verification. It provides a simple command-line interface to help users quickly compute file hashes and verify file integrity.

## Features

- Compute various file hashes (MD5, SHA1, SHA256, SHA384, SHA512)
- Verify if a file matches a specified hash
- Support both hexadecimal and Base64 formatted hash output
- Simple command-line interface for easy operation

## Tech Stack

- **Language**: C# 14.0+
- **Runtime**: .NET 10 or higher
- **Framework**: ConsoleAppFramework
- **Cryptography**: System.Security.Cryptography
- **Testing Framework**: MSTest

## Installation and Usage

### Clone the Project

```bash
git clone https://github.com/your-repo/fuse.git
cd fuse
```

### Run the Project

```bash
dotnet run --project src/Fuse.csproj [commands] [options]
```

## Commands

### hash - Calculate File Hash

Calculate the hash of a specified file.

```bash
dotnet run -- hash -p <file path> -a <algorithm> -b
```

Parameters:
- `-p` or `--filePath`: Path of the file to calculate hash for
- `-a` or `--algorithm`: Hash algorithm (optional, defaults to sha256)
- `-b` or `--base64`: Output in Base64 format (optional)

Example:
```bash
# Using the default SHA256 algorithm
dotnet run -- hash -p ./example.txt

# Using MD5 algorithm
dotnet run -- hash -p ./example.txt -a md5

# Output in Base64 format
dotnet run -- hash -p ./example.txt -a sha256 -b
```

### verify - Verify File Hash

Verify if a specified file matches the expected hash value.

```bash
dotnet run -- verify -p <file path> -e <expected hash> -a <algorithm> -b
```

Parameters:
- `-p` or `--filePath`: Path of the file to verify
- `-e` or `--expectedHash`: Expected hash value
- `-a` or `--algorithm`: Hash algorithm (optional, defaults to sha256)
- `-b` or `--isBase64`: Hash is in Base64 format (optional)

Example:
```bash
# Verify if file matches the specified hash
dotnet run -- verify -p ./example.txt -e abcdef123456...

# Verify Base64 formatted hash
dotnet run -- verify -p ./example.txt -e someBase64String -b
```

## Testing

Run unit tests:

```bash
dotnet test
```

## License

[MIT](./LICENSE.txt)