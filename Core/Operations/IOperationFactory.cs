namespace SevenZipSharpArchiver.Core.Operations
{
    /// <summary>
    /// Interface for creating operation instances
    /// </summary>
    public interface IOperationFactory
    {
        /// <summary>
        /// Creates an operation based on the specified type
        /// </summary>
        /// <param name="type">Operation type</param>
        /// <param name="profileName">Optional compression profile name</param>
        /// <returns>The created operation</returns>
        IOperation CreateOperation(OperationType type, string profileName = null);
    }
} 