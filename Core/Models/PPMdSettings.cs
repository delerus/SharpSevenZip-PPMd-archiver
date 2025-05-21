using System;
using SharpSevenZip;

namespace SevenZipSharpArchiver.Core.Models
{
    /// <summary>
    /// Settings model for PPMd compression
    /// </summary>
    public class PPMdSettings
    {
        /// <summary>
        /// PPMd model order (context length)
        /// Valid range: 2-16
        /// </summary>
        public int ModelOrder { get; set; } = 16;

        /// <summary>
        /// Memory usage limit in MB
        /// Valid range: 1-2048
        /// </summary>
        public int MemorySizeMB { get; set; } = 2048;

        /// <summary>
        /// Text complexity type
        /// </summary>
        public TextComplexity Complexity { get; set; } = TextComplexity.Medium;

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

    /// <summary>
    /// Text complexity categories
    /// </summary>
    public enum TextComplexity
    {
        /// <summary>
        /// Low complexity text (logs, JSON)
        /// </summary>
        Low,
        
        /// <summary>
        /// Medium complexity text (plain text)
        /// </summary>
        Medium,
        
        /// <summary>
        /// High complexity text (source code)
        /// </summary>
        High
    }
}
