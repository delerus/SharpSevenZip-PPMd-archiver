# 7zSharpArchiver API

This module provides a programming interface (API) for integrating archiving and extraction functionality into other applications.

## Key Features

- Compression of a single file into an archive
- Compression of multiple files into an archive
- Extraction of an archive to a specified directory
- Extraction of multiple archives simultaneously
- Automatic operation detection (compression or decompression)
- Synchronous and asynchronous operations
- Error handling and detailed operation results

## Using the API

### Initialization

```csharp
// Create API with default logging
var api = new ArchiveApi();

// Or with a custom logger factory
var loggerFactory = new DefaultLoggerFactory("path/to/logs");
var api = new ArchiveApi(loggerFactory);
```

### Automatic Operation Detection

```csharp
// Let the API automatically determine if compression or decompression is needed
var inputFiles = new List<string> { @"C:\temp\document.txt" };
string outputPath = @"C:\temp\document.7z";

// Synchronous operation
ArchiveResult result = api.ProcessAutomatic(inputFiles, outputPath);

// Asynchronous operation
ArchiveResult result = await api.ProcessAutomaticAsync(inputFiles, outputPath);

if (result.Success)
{
    Console.WriteLine("Operation completed successfully");
}
else
{
    Console.WriteLine($"Operation error: {result.Message}");
}

// Automatic decompression example
var archiveFile = new List<string> { @"C:\temp\archive.7z" };
string extractPath = @"C:\temp\extracted";
Directory.CreateDirectory(extractPath);

result = api.ProcessAutomatic(archiveFile, extractPath);
```

### Compressing a Single File

```csharp
string inputFile = @"C:\temp\document.txt";
string outputFile = @"C:\temp\document.7z";

// Synchronous operation
ArchiveResult result = api.CompressFile(inputFile, outputFile, "text");

// Asynchronous operation
ArchiveResult result = await api.CompressFileAsync(inputFile, outputFile, "text");

if (result.Success)
{
    Console.WriteLine("File compressed successfully");
}
else
{
    Console.WriteLine($"Compression error: {result.Message}");
}
```

### Compressing Multiple Files

```csharp
var inputFiles = new List<string>
{
    @"C:\temp\file1.txt",
    @"C:\temp\file2.txt",
    @"C:\temp\file3.txt"
};
string outputFile = @"C:\temp\archive.7z";

ArchiveResult result = api.CompressFiles(inputFiles, outputFile, "logs");

if (result.Success)
{
    Console.WriteLine("Files compressed successfully");
}
```

### Extracting an Archive

```csharp
string archiveFile = @"C:\temp\archive.7z";
string outputDirectory = @"C:\temp\extracted";

// Create directory if it doesn't exist
Directory.CreateDirectory(outputDirectory);

// Synchronous operation
ArchiveResult result = api.DecompressArchive(archiveFile, outputDirectory);

// Asynchronous operation
ArchiveResult result = await api.DecompressArchiveAsync(archiveFile, outputDirectory);

if (result.Success)
{
    Console.WriteLine("Archive extracted successfully");
}
```

### Extracting Multiple Archives

```csharp
var archiveFiles = new List<string>
{
    @"C:\temp\archive1.7z",
    @"C:\temp\archive2.zip",
    @"C:\temp\archive3.rar"
};
string outputDirectory = @"C:\temp\extracted_multiple";

// Create directory if it doesn't exist
Directory.CreateDirectory(outputDirectory);

// Synchronous operation
ArchiveResult result = api.DecompressArchives(archiveFiles, outputDirectory);

// Asynchronous operation
ArchiveResult result = await api.DecompressArchivesAsync(archiveFiles, outputDirectory);

if (result.Success)
{
    Console.WriteLine("All archives extracted successfully");
}
else
{
    Console.WriteLine($"Extraction error: {result.Message}");
}
```

When extracting multiple archives, each archive is extracted to its own subdirectory named after the archive file (without extension) inside the output directory.

## Modularity and Extensibility

The API is designed with modularity and extensibility in mind, following SOLID principles:

### Dependency Injection

The API supports dependency injection through its constructor:

```csharp
// Create with custom dependencies
var api = new ArchiveApi(customLoggerFactory);
```

### Operation Detection

The API includes an operation detection mechanism that automatically determines whether to compress or decompress based on input files and output path:

```csharp
// The API uses DefaultOperationDetector internally
var operationDetector = new DefaultOperationDetector(logger);
var operationType = operationDetector.DetectOperation(inputFiles, outputPath);

// This is used by the ProcessAutomatic method
```

### Integration with Core Architecture

The API seamlessly integrates with the core architecture through the `ArchiverManagerBuilder`:

```csharp
// Example of how the API uses the builder pattern internally
var archiver = new ArchiverManagerBuilder()
    .WithInputFile(inputFile)
    .WithOutputPath(outputFile)
    .WithProfile(profileName)
    .WithLoggerFactory(_loggerFactory)
    .Build();
```

### Extending the API

To extend the API with new functionality:

1. Add new methods to the `ArchiveApi` class
2. Create new operation types in the core `OperationType` enum
3. Implement new operations by creating classes that implement `IOperation`
4. Register the new operations in the `DefaultOperationFactory`

## Logging

The API includes comprehensive logging capabilities:

### Logging Configuration

```csharp
// Create a custom logger factory
var loggerFactory = new DefaultLoggerFactory("logs/archive_operations.log");
var api = new ArchiveApi(loggerFactory);
```

### Log Events

The API logs the following events:

- Operation start (Information level)
- Operation completion (Information level)
- Operation failures (Error level)
- Detailed exception information

### Custom Logging

You can implement your own `ILoggerFactory` and `ILogger` to integrate with your application's logging system:

```csharp
public class CustomLoggerFactory : ILoggerFactory
{
    public ILogger CreateLogger(string name)
    {
        // Return your custom logger implementation
        return new CustomLogger(name);
    }
}

// Then use it with the API
var api = new ArchiveApi(new CustomLoggerFactory());
```

## Compression Profiles

The API supports the following built-in compression profiles:

- `text` - optimized for regular text files
- `logs` - optimized for logs and structured data
- `extreme` - maximum compression (slower)

If no profile is specified, it will be automatically selected based on the file type.

## Handling Results

The API returns `ArchiveResult` objects containing information about the operation result:

```csharp
public class ArchiveResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public Exception Exception { get; set; }
}
```

For additional compression information, you can use `ArchiveFileResult`:

```csharp
public class ArchiveFileResult : ArchiveResult
{
    public List<string> ProcessedFiles { get; set; }
    public long TotalSizeBytes { get; set; }
    public long CompressedSizeBytes { get; set; }
    public double CompressionRatio { get; }
}
```

## Usage Examples

See the `ApiExample` class for complete examples of using the API, including:

- Compressing a single file
- Compressing multiple files
- Decompressing an archive
- Using asynchronous operations
- Using automatic operation detection 