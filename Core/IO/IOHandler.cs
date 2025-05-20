using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenZipSharpArchiver.Core.IO
{
    public static class IOHandler
    {
        public static void ValidateReadFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Путь к файлу не может быть пустым");

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Файл не найден: {filePath}");

            try
            {
                Path.GetFullPath(filePath);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Некорректный путь: {filePath}", ex);
            }
        }

        public static void ValidateWriteFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Путь к файлу не может быть пустым");

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
                        throw new ArgumentException($"Не удалось создать директорию: {directory}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Некорректный путь: {filePath}", ex);
            }
        }
    }
}
