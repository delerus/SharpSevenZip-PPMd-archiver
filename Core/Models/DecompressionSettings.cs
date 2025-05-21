using System;
using SharpSevenZip;

namespace SevenZipSharpArchiver.Core.Models
{
    /// <summary>
    /// Settings model for archive decompression
    /// </summary>
    public class DecompressionSettings
    {
        /// <summary>
        /// Whether to preserve directory structure during extraction
        /// </summary>
        public bool PreserveDirectoryStructure { get; set; } = true;

        /// <summary>
        /// Password for encrypted archives
        /// </summary>
        public string Password { get; set; } = null;

        /// <summary>
        /// Whether to overwrite existing files
        /// </summary>
        public bool OverwriteExisting { get; set; } = true;

        /// <summary>
        /// Whether to validate archive integrity before extraction
        /// </summary>
        public bool ValidateArchiveIntegrity { get; set; } = false;
    }
} 