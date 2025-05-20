using SharpSevenZip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenZipSharpArchiver.Core.Compression
{
    public interface IDecompressor
    {
        void DecompressFile(string inputFilePath, string outputFilePath);
    }
}
