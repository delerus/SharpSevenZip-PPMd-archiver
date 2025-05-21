using System;
using System.Collections.Generic;
using SevenZipSharpArchiver.Core.Models;
using SharpSevenZip;

namespace SevenZipSharpArchiver.Core.Configuration
{
    /// <summary>
    /// Configurator for SharpSevenZipCompressor
    /// </summary>
    public static class CompressorConfigurator
    {
        /// <summary>
        /// Configures SharpSevenZipCompressor with PPMd settings
        /// </summary>
        /// <param name="compressor">Compressor instance to configure</param>
        /// <param name="settings">PPMd settings model</param>
        /// <param name="compressionParams">Custom compression parameters</param>
        /// <returns>Configured compressor</returns>
        public static SharpSevenZipCompressor Configure(
            SharpSevenZipCompressor compressor, 
            PPMdSettings settings,
            Dictionary<string, string> compressionParams)
        {
            if (compressor == null)
                throw new ArgumentNullException(nameof(compressor));
            
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            // Basic configuration from settings model
            compressor.ArchiveFormat = settings.ArchiveFormat;
            compressor.CompressionMethod = settings.CompressionMethod;
            compressor.CompressionLevel = settings.CompressionLevel;

            // Directory settings from settings model
            compressor.FastCompression = settings.FastCompression;
            compressor.DirectoryStructure = settings.DirectoryStructure;
            compressor.IncludeEmptyDirectories = settings.IncludeEmptyDirectories;
            compressor.PreserveDirectoryRoot = settings.PreserveDirectoryRoot;

            // Apply custom parameters
            if (compressionParams != null)
            {
                foreach (var setting in compressionParams)
                {
                    if (!compressor.CustomParameters.ContainsKey(setting.Key))
                    {
                        compressor.CustomParameters.Add(setting.Key, setting.Value);
                    }
                }
            }

            return compressor;
        }
    }
} 