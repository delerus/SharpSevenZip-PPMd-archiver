using System;
using SevenZipSharpArchiver.Core.Models;
using SharpSevenZip;

namespace SevenZipSharpArchiver.Core.Compression
{
    internal class Compressor : ICompressor
    {
        private readonly Dictionary<string, string> _compressionParams;
        private SharpSevenZipCompressor _compressor;

        public Compressor(Dictionary<string, string> compressionParams)
        {
            _compressionParams = compressionParams;
        }

        public void CompressFile(string inputFilePath, string outputFilePath)
        {
            try
            {
                _compressor = new SharpSevenZipCompressor();

                _compressor.ArchiveFormat = OutArchiveFormat.SevenZip;
                _compressor.CompressionMethod = CompressionMethod.Ppmd;
                _compressor.CompressionLevel = CompressionLevel.Ultra;

                _compressor.FastCompression = false;
                _compressor.DirectoryStructure = true;
                _compressor.IncludeEmptyDirectories = false;
                _compressor.PreserveDirectoryRoot = false;

                foreach (var setting in _compressionParams)
                {
                    if (!_compressor.CustomParameters.ContainsKey(setting.Key))
                    {
                        _compressor.CustomParameters.Add(setting.Key, setting.Value);
                    }
                }

                _compressor.CompressFiles(outputFilePath, inputFilePath);

            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при сжатии файла: {ex.Message}", ex);
            }
        }
    }
}
