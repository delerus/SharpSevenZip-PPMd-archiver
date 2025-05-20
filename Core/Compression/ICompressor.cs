using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenZipSharpArchiver.Core.Compression
{
    public interface ICompressor
    {
        void CompressFile(string inputFilePath, string outputFilePath);
    }
}
