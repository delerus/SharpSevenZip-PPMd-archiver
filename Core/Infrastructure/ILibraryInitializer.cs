namespace SevenZipSharpArchiver.Core.Infrastructure
{
    /// <summary>
    /// Interface for initializing external libraries
    /// </summary>
    public interface ILibraryInitializer
    {
        /// <summary>
        /// Initializes the library
        /// </summary>
        void Initialize();
    }
} 