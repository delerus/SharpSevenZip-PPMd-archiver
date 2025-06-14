using System;
using SharpSevenZip;

namespace SevenZipSharpArchiver.Core.Models
{
    /// <summary>
    /// Settings model for PPMd compression
    /// </summary>
    public class PPMdSettings
    {
        private int _modelOrder = 10;
        /// <summary>
        /// PPMd model order (context length)
        /// Valid range: 2-16
        /// </summary>
        public int ModelOrder
        {
            get => _modelOrder;
            set
            {
                if (value < 1 || value > 16)
                    throw new ArgumentOutOfRangeException(nameof(ModelOrder), "ModelOrder must be between 1 and 16.");
                _modelOrder = value;
            }
        }

        private int _memorySizeMB = 2048;
        /// <summary>
        /// Memory usage limit in MB
        /// Valid range: 1-2048
        /// </summary>
        public int MemorySizeMB
        {
            get => _memorySizeMB;
            set
            {
                if (value < 1 || value > 2048)
                    throw new ArgumentOutOfRangeException(nameof(MemorySizeMB), "MemorySizeMB must be between 1 and 2048.");
                _memorySizeMB = value;
            }
        }

        /// <summary>
        /// Archive format to use
        /// </summary>
        public OutArchiveFormat ArchiveFormat { get; set; } = OutArchiveFormat.SevenZip;

        /// <summary>
        /// Compression method to use
        /// </summary>
        public CompressionMethod CompressionMethod { get; set; } = CompressionMethod.Ppmd;

        /// <summary>
        /// Compression level
        /// </summary>
        public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Ultra;

        /// <summary>
        /// Whether to use fast compression
        /// </summary>
        public bool FastCompression { get; set; } = false;

        /// <summary>
        /// Whether to preserve directory structure
        /// </summary>
        public bool DirectoryStructure { get; set; } = true;

        /// <summary>
        /// Whether to include empty directories
        /// </summary>
        public bool IncludeEmptyDirectories { get; set; } = false;

        /// <summary>
        /// Whether to preserve directory root
        /// </summary>
        public bool PreserveDirectoryRoot { get; set; } = false;
    }
}
