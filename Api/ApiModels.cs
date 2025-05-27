using System.Collections.Generic;

namespace SevenZipSharpArchiver.Api
{
    /// <summary>
    /// Represents a request to compress files
    /// </summary>
    public class CompressRequest
    {
        /// <summary>
        /// Gets or sets the input file paths
        /// </summary>
        public List<string> InputFiles { get; set; } = new List<string>();
        
        /// <summary>
        /// Gets or sets the output archive path
        /// </summary>
        public string OutputPath { get; set; }
        
        /// <summary>
        /// Gets or sets the compression profile name
        /// </summary>
        public string ProfileName { get; set; }
    }
    
    /// <summary>
    /// Represents a request to decompress an archive
    /// </summary>
    public class DecompressRequest
    {
        /// <summary>
        /// Gets or sets the archive file path
        /// </summary>
        public string ArchiveFile { get; set; }
        
        /// <summary>
        /// Gets or sets the output directory path
        /// </summary>
        public string OutputDirectory { get; set; }
    }
    
    /// <summary>
    /// Represents compression options
    /// </summary>
    public class CompressionOptions
    {
        /// <summary>
        /// Gets or sets the compression level (1-9)
        /// </summary>
        public int CompressionLevel { get; set; } = 5;
        
        /// <summary>
        /// Gets or sets the dictionary size in MB
        /// </summary>
        public int DictionarySizeMB { get; set; } = 16;
        
        /// <summary>
        /// Gets or sets whether to use solid compression
        /// </summary>
        public bool UseSolidCompression { get; set; } = true;
        
        /// <summary>
        /// Gets or sets whether to include file names in the archive
        /// </summary>
        public bool IncludeFileNames { get; set; } = true;
    }
} 