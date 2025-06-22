using System;
using System.Collections.Generic;
using SevenZipSharpArchiver.Core.Models;
using SharpSevenZip;

namespace SevenZipSharpArchiver.Core.Profiling
{
    /// <summary>
    /// Compression profile types
    /// </summary>
    public enum CompressionProfileType
    {
        /// <summary>
        /// Optimized for logs, JSON, XML, and simple structured data
        /// </summary>
        Logs,
        
        /// <summary>
        /// Optimized for regular text documents
        /// </summary>
        Text,
        
        /// <summary>
        /// Maximum compression, only selected manually
        /// </summary>
        Extreme
    }

    /// <summary>
    /// Contains compression profile definitions and settings
    /// </summary>
    public static class Profiles
    {
        private static readonly Dictionary<string, CompressionProfileType> ProfileNameMap = 
            new Dictionary<string, CompressionProfileType>(StringComparer.OrdinalIgnoreCase)
        {
            { "logs", CompressionProfileType.Logs },
            { "text", CompressionProfileType.Text },
            { "extreme", CompressionProfileType.Extreme },
        };
        
        /// <summary>
        /// Gets compression profile type from its name (for CLI processing)
        /// </summary>
        /// <param name="profileName">Name of the profile (case insensitive)</param>
        /// <param name="defaultProfile">Default profile to use if name is not recognized</param>
        /// <returns>Compression profile type</returns>
        public static CompressionProfileType GetProfileByName(string profileName, CompressionProfileType defaultProfile = CompressionProfileType.Text)
        {
            if (string.IsNullOrWhiteSpace(profileName))
                return defaultProfile;
                
            if (ProfileNameMap.TryGetValue(profileName.Trim(), out var profileType))
                return profileType;
                
            return defaultProfile;
        }
        
        /// <summary>
        /// Gets optimal PPMd settings for the specified compression profile
        /// </summary>
        /// <param name="profile">Compression profile</param>
        /// <returns>Optimized PPMd settings</returns>
        public static PPMdSettings GetSettings(CompressionProfileType profile)
        {
            return profile switch
            {
                CompressionProfileType.Logs => GetLogsProfileSettings(),
                CompressionProfileType.Text => GetTextProfileSettings(),
                CompressionProfileType.Extreme => GetExtremeProfileSettings(),
                _ => GetTextProfileSettings() // Default to Text profile
            };
        }

        /// <summary>
        /// Settings for Logs profile (fast compression for simple data)
        /// </summary>
        private static PPMdSettings GetLogsProfileSettings()
        {
            return new PPMdSettings
            {
                ModelOrder = 5,               // Small context for simple data
                MemorySizeMB = 64,            // Less memory for simple data
                CompressionLevel = CompressionLevel.Fast,
                FastCompression = true
            };
        }

        /// <summary>
        /// Settings for Text profile (standard text and documents)
        /// </summary>
        private static PPMdSettings GetTextProfileSettings()
        {
            return new PPMdSettings
            {
                ModelOrder = 7,               // Optimized context for text documents
                MemorySizeMB = 256,           // Optimized memory usage for text
                CompressionLevel = CompressionLevel.Normal,
                FastCompression = false
            };
        }

        /// <summary>
        /// Settings for Extreme profile (using text profile settings as requested)
        /// </summary>
        private static PPMdSettings GetExtremeProfileSettings()
        {
            return new PPMdSettings
            {
                ModelOrder = 10,              // Using previous text profile settings
                MemorySizeMB = 1024,          // Using previous text profile settings
                CompressionLevel = CompressionLevel.Ultra,
                FastCompression = false
            };
        }
    }
} 