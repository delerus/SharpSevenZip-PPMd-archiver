using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SevenZipSharpArchiver.Core.Models;
using SevenZipSharpArchiver.Core.Compression;
using SevenZipSharpArchiver.Core.IO;
using SevenZipSharpArchiver.Core.Configuration;
using SevenZipSharpArchiver.Core.Logging;

namespace SevenZipSharpArchiver.Core
{
    public class ArchiverManager
    {
        private PPMdSettings _settings;
        private string _inputFilePath;
        private string _outputFilePath;

        string[] archiveExtensions = { ".7z", ".zip", ".rar", ".tar", ".gz", ".bz2", ".xz", ".cab", ".iso" };

        public ArchiverManager(string inputFile, string outputFile)
        {
            _settings = new PPMdSettings();
            _inputFilePath = inputFile;
            _outputFilePath = outputFile;
        }

        public void Init()
        {
            try
            {
                IOHandler.ValidateReadFile(_inputFilePath);

                string fileExtension = Path.GetExtension(_inputFilePath).ToLower();

                BaseLibraryLoader.InitializeLibrary();

                if (archiveExtensions.Contains(fileExtension))
                {
                    Decompression();
                }
                else
                {
                    IOHandler.ValidateWriteFile(_outputFilePath);
                    Compression();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }


        }

        public void Compression()
        {
            var compressionParams = PPMdSettingsConverter.ToSharpSevenZipParams(_settings);
            var compressor = new Compressor(compressionParams);

            ConsoleWriter.Init(_inputFilePath, _outputFilePath);
            ConsoleWriter.StartTimer();

            try
            {
                compressor.CompressFile(_inputFilePath, _outputFilePath);
                ConsoleWriter.StopTimerAndWriteInfo(OperationType.Compression);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка во время сжатия: {ex.Message}");
            }

        }

        public void Decompression()
        {
            var decompressor = new Decompressor();

            ConsoleWriter.Init(_inputFilePath, _outputFilePath);
            ConsoleWriter.StartTimer();

            try
            {
                decompressor.DecompressFile(_inputFilePath, _outputFilePath);
                ConsoleWriter.StopTimerAndWriteInfo(OperationType.Decompression);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка распаковки: {ex.Message}");
            }
        }
    }
}
