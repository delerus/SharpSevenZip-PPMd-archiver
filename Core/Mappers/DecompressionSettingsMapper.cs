using System;
using System.Collections.Generic;
using SevenZipSharpArchiver.Core.Models;

namespace SevenZipSharpArchiver.Core.Mappers
{
    /// <summary>
    /// Mapper for decompression settings
    /// </summary>
    public class DecompressionSettingsMapper : ICompressionSettingsMapper<DecompressionSettings>
    {
        /// <summary>
        /// Maps DecompressionSettings to dictionary of parameters
        /// </summary>
        /// <param name="settings">Decompression settings</param>
        /// <returns>Dictionary with parameters</returns>
        public Dictionary<string, string> MapToParameters(DecompressionSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            var parameters = new Dictionary<string, string>();

            // Currently no additional parameters
            // used to future extensibility
            
            return parameters;
        }
    }
} 