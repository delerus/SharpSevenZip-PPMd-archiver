using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SevenZipSharpArchiver.Core.Logging;
using SevenZipSharpArchiver.Core.Models;

namespace SevenZipSharpArchiver.Core.Profiling
{
    /// <summary>
    /// Selects and applies compression profiles
    /// </summary>
    public class ProfileSelector
    {
        private readonly ILogger _logger;
        
        // Collections of file extensions for Logs profile
        private readonly HashSet<string> _logsProfileExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            // Log files
            ".log", ".txt",
            // Structured data
            ".json", ".xml", ".csv", ".tsv", ".yml", ".yaml",
            // Configuration files
            ".ini", ".config", ".properties", ".env"
        };
        
        public ProfileSelector(ILogger logger = null)
        {
            _logger = logger;
        }
        
        /// <summary>
        /// Auto-selects profile based on file extension
        /// </summary>
        /// <param name="filePath">Path to the file</param>
        /// <returns>Selected profile type</returns>
        public CompressionProfileType AutoSelectProfile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                _logger?.Warning("Empty file path provided, using default Text profile");
                return CompressionProfileType.Text;
            }
            
            string extension = Path.GetExtension(filePath).ToLowerInvariant();
            
            if (string.IsNullOrEmpty(extension))
            {
                _logger?.Debug($"File has no extension: {filePath}, using default Text profile");
                return CompressionProfileType.Text;
            }
            
            if (_logsProfileExtensions.Contains(extension))
            {
                _logger?.Debug($"File extension {extension} mapped to Logs profile");
                return CompressionProfileType.Logs;
            }
            
            _logger?.Debug($"File extension {extension} not specifically mapped, using default Text profile");
            return CompressionProfileType.Text;
        }
        
        /// <summary>
        /// Applies profile settings to the PPMd settings
        /// </summary>
        /// <param name="settings">PPMd settings to modify</param>
        /// <param name="profileType">Profile type to apply</param>
        /// <returns>Modified settings</returns>
        public PPMdSettings ApplyProfile(PPMdSettings settings, CompressionProfileType profileType)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));
                
            _logger?.Information($"Applying {profileType} profile to settings");
            
            // Get optimized settings for this profile
            var profileSettings = Profiles.GetSettings(profileType);
            
            // Apply profile settings to the provided settings object
            settings.ModelOrder = profileSettings.ModelOrder;
            settings.MemorySizeMB = profileSettings.MemorySizeMB;
            settings.CompressionLevel = profileSettings.CompressionLevel;
            settings.FastCompression = profileSettings.FastCompression;
            
            return settings;
        }
        
        /// <summary>
        /// Selects appropriate profile and applies settings
        /// </summary>
        /// <param name="settings">Settings to update</param>
        /// <param name="filePath">File path (for auto-detection)</param>
        /// <param name="profileName">Optional profile name from CLI</param>
        /// <returns>Updated settings</returns>
        public PPMdSettings SelectAndApplyProfile(PPMdSettings settings, string filePath, string profileName = null)
        {
            CompressionProfileType profileType;
            
            // If profile specified in CLI, use it
            if (!string.IsNullOrWhiteSpace(profileName))
            {
                profileType = Profiles.GetProfileByName(profileName);
                _logger?.Information($"Using profile '{profileName}' specified in command line");
            }
            // Otherwise auto-select based on file extension
            else
            {
                profileType = AutoSelectProfile(filePath);
                _logger?.Information($"Auto-selected profile {profileType} based on file extension");
            }
            
            // Apply the profile settings
            return ApplyProfile(settings, profileType);
        }
    }
} 