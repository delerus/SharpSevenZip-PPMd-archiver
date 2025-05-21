using System;
using SharpSevenZip;
using SevenZipSharpArchiver.Core.Models;

namespace SevenZipSharpArchiver.Core.Configuration
{
    /// <summary>
    /// Configurator for SharpSevenZipExtractor
    /// </summary>
    public static class DecompressorConfigurator
    {
        /// <summary>
        /// Configures SharpSevenZipExtractor with decompression settings
        /// </summary>
        /// <param name="extractor">Extractor instance to configure</param>
        /// <param name="settings">Decompression settings</param>
        /// <returns>Configured extractor</returns>
        public static SharpSevenZipExtractor Configure(
            SharpSevenZipExtractor extractor,
            DecompressionSettings settings)
        {
            if (extractor == null)
                throw new ArgumentNullException(nameof(extractor));

            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            // Placeholder for future extensibility
            // No logic yet

            return extractor;
        }
    }
} 