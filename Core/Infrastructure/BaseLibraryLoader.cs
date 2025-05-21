using System;
using System.IO;
using System.Reflection;
using SharpSevenZip;

namespace SevenZipSharpArchiver.Core.Infrastructure
{
    /// <summary>
    /// Handles loading of the 7z native library
    /// </summary>
    public static class BaseLibraryLoader
    {
        private static bool _isInitialized = false;
        private static readonly object _lockObject = new object();

        /// <summary>
        /// Initializes the 7z library
        /// </summary>
        public static void InitializeLibrary()
        {
            if (_isInitialized)
                return;

            lock (_lockObject)
            {
                if (_isInitialized)
                    return;

                try
                {
                    string dllPath = GetDllPath();
                    
                    if (!File.Exists(dllPath))
                        throw new FileNotFoundException($"7z.dll not found at {dllPath}");

                    SharpSevenZipBase.SetLibraryPath(dllPath);
                    _isInitialized = true;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error initializing 7z library: {ex.Message}", ex);
                }
            }
        }

        private static string GetDllPath()
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string directory = Path.GetDirectoryName(assemblyLocation);
            return Path.Combine(directory, "7z.dll");
        }
    }
} 