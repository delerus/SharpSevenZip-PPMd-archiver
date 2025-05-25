using System;
using System.IO;

namespace SevenZipSharpArchiver.Core.IO
{
    /// <summary>
    /// Validates file paths for reading and writing operations
    /// </summary>
    public static class FilePathValidator
    {
        /// <summary>
        /// Validates the path to a file that will be read
        /// </summary>
        /// <param name="filePath">The path to the file to validate</param>
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

        /// <summary>
        /// Validates the path to a file that will be written
        /// </summary>
        /// <param name="filePath">The path to the file to validate</param>
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
