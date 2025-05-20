using SharpSevenZip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenZipSharpArchiver.Core.Compression
{
    internal class Decompressor : IDecompressor
    {
        private SharpSevenZipExtractor _decompressor;

        public void DecompressFile(string inputFilePath, string outputFilePath)
        {
            try
            {
                _decompressor = new SharpSevenZipExtractor(inputFilePath);
                _decompressor.ExtractArchive(outputFilePath);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при распаковки файла: {ex.Message}", ex);
            }
            finally 
            { 
                _decompressor?.Dispose(); 
            }
        }
    }
}
