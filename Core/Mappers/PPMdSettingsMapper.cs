using SevenZipSharpArchiver.Core.Models;
using System;
using System.Collections.Generic;

namespace SevenZipSharpArchiver.Core.Mappers
{
    /// <summary>
    /// Mapper for PPMd settings to SharpSevenZip parameters
    /// </summary>
    public class PPMdSettingsMapper : ICompressionSettingsMapper<PPMdSettings>
    {
        /// <summary>
        /// Maps PPMdSettings model to dictionary of parameters for SharpSevenZip
        /// </summary>
        /// <param name="settings">PPMd settings model</param>
        /// <returns>Dictionary with SharpSevenZip parameters</returns>
        public Dictionary<string, string> MapToParameters(PPMdSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            var parameters = new Dictionary<string, string>
            {
                { "o", settings.ModelOrder.ToString() },      // Model order (2-16)
                { "mem", $"{settings.MemorySizeMB}m" },       // Memory size in MB (1-2048)
            };

            return parameters;
        }

        /// <summary>
        /// Legacy method for backward compatibility
        /// </summary>
        /// <param name="settings">PPMd settings model</param>
        /// <returns>Dictionary with SharpSevenZip parameters</returns>
        [Obsolete("Use MapToParameters instead")]
        public static Dictionary<string, string> ToSharpSevenZipParams(PPMdSettings settings)
        {
            return new PPMdSettingsMapper().MapToParameters(settings);
        }
    }
} 