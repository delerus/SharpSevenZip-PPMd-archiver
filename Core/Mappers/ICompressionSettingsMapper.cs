using System;
using System.Collections.Generic;

namespace SevenZipSharpArchiver.Core.Mappers
{
    /// <summary>
    /// Generic interface for compression settings mappers
    /// </summary>
    /// <typeparam name="TSettings">Type of settings model</typeparam>
    public interface ICompressionSettingsMapper<TSettings>
    {
        /// <summary>
        /// Maps settings model to dictionary of parameters for compression libraries
        /// </summary>
        /// <param name="settings">Settings model</param>
        /// <returns>Dictionary with compression parameters</returns>
        Dictionary<string, string> MapToParameters(TSettings settings);
    }
} 