namespace ProductRepositoryAsync
{
    /// <summary>
    /// Represents a result of a database operation.
    /// </summary>
    public enum OperationResult
    {
        /// <summary>
        /// Operation completed successfully.
        /// </summary>
        Success = 0,

        /// <summary>
        /// Operation is not completed because of connection issues.
        /// </summary>
        ConnectionIssue = 1,

        /// <summary>
        /// Operation is not started because the specified collection name is invalid.
        /// </summary>
        InvalidCollectionName = 2,

        /// <summary>
        /// Operation is not completed because of unknown error.
        /// </summary>
        Failure = -1,
    }
}
