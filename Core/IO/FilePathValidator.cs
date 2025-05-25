using System;
using System.IO;

namespace SevenZipSharpArchiver.Core.IO
{
    public static class FilePathValidator
    {
        public static void ValidateReadFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Path to file cannot be empty");

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found: {filePath}");

            try
            {
                Path.GetFullPath(filePath);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Invalid path: {filePath}", ex);
            }
        }

        public static void ValidateWriteFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Path to file cannot be empty");

            try
            {
                string fullPath = Path.GetFullPath(filePath);
                string directory = Path.GetDirectoryName(fullPath);

                if (!Directory.Exists(directory))
                {
                    try
                    {
                        Directory.CreateDirectory(directory);
                    }
                    catch
                    {
                        throw new ArgumentException($"Failed to create directory: {directory}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Invalid path: {filePath}", ex);
            }
        }
    }
}
