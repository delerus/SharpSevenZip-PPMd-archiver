using System;
using System.IO;
using System.Reflection;
using SharpSevenZip;
using SevenZipSharpArchiver.Core.Logging;

namespace SevenZipSharpArchiver.Core.Infrastructure
{
    /// <summary>
    /// Handles initialization of the 7z native library
    /// </summary>
    public class SevenZipLibraryInitializer : ILibraryInitializer
    {
        private readonly ILogger _logger;
        private bool _isInitialized = false;
        private readonly object _lockObject = new object();

        /// <summary>
        /// Creates a new instance of SevenZipLibraryInitializer
        /// </summary>
        /// <param name="logger">Logger instance</param>
        public SevenZipLibraryInitializer(ILogger logger = null)
        {
            _logger = logger;
        }

        /// <summary>
        /// Initializes the 7z library
        /// </summary>
        public void Initialize()
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
                    _logger?.Debug($"Initializing 7z library from: {dllPath}");
                    
                    if (!File.Exists(dllPath))
                    {
                        _logger?.Error($"7z.dll not found at {dllPath}");
                        throw new FileNotFoundException($"7z.dll not found at {dllPath}");
                    }

                    SharpSevenZipBase.SetLibraryPath(dllPath);
                    _isInitialized = true;
                    _logger?.Information("7z library successfully initialized");
                }
                catch (Exception ex)
                {
                    _logger?.Error("Error initializing 7z library", ex);
                    throw new Exception($"Error initializing 7z library: {ex.Message}", ex);
                }
            }
        }

        private string GetDllPath()
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string directory = Path.GetDirectoryName(assemblyLocation);
            return Path.Combine(directory, "7z.dll");
        }
    }
} 