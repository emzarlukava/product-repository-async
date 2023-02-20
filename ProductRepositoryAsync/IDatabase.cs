namespace ProductRepositoryAsync
{
    /// <summary>
    /// Represents a simple document database for storing collections and collection elements.
    /// </summary>
    public interface IDatabase
    {
        /// <summary>
        /// Returns true, if the database has a collection with the specified name; otherwise false.
        /// </summary>
        /// <param name="collectionName">A collection name.</param>
        /// <param name="collectionExists">A value indicating the database has a collection with the specified name.</param>
        /// <returns>An operation result.</returns>
        Task<OperationResult> IsCollectionExistAsync(string collectionName, out bool collectionExists);

        /// <summary>
        /// Returns true, if the database has a collection element with specified <paramref name="id"/>; otherwise false.
        /// </summary>
        /// <param name="collectionName">A collection name.</param>
        /// <param name="id">An identifier of a collection element.</param>
        /// <param name="collectionElementExists">A value indicating the database has a collection element with the specified <paramref name="id"/>.</param>
        /// <returns>An operation result.</returns>
        Task<OperationResult> IsCollectionElementExistAsync(string collectionName, int id, out bool collectionElementExists);

        /// <summary>
        /// Creates a collection with the specified <paramref name="collectionName"/>.
        /// </summary>
        /// <param name="collectionName">A collection name.</param>
        /// <returns>An operation result.</returns>
        Task<OperationResult> CreateCollectionAsync(string collectionName);

        /// <summary>
        /// Generates a unique identifier for a collection element in the specified collection.
        /// </summary>
        /// <param name="collectionName">A collection name.</param>
        /// <param name="id">A generated unique identifier.</param>
        /// <returns>An operation result.</returns>
        Task<OperationResult> GenerateIdAsync(string collectionName, out int id);

        /// <summary>
        /// Gets a collection element with the specified <paramref name="id"/> from the collection with the specified <paramref name="collectionName"/>.
        /// </summary>
        /// <param name="collectionName">A collection name.</param>
        /// <param name="id">A unique identifier of a collection element to search in the collection.</param>
        /// <param name="data">A collection element data.</param>
        /// <returns>An operation result.</returns>
        Task<OperationResult> GetCollectionElementAsync(string collectionName, int id, out IDictionary<string, string> data);

        /// <summary>
        /// Inserts a new collection element to the collection with the specified <paramref name="collectionName"/>.
        /// </summary>
        /// <param name="collectionName">A collection name.</param>
        /// <param name="id">A unique identifier of a collection element to insert to the collection.</param>
        /// <param name="data">A collection element data.</param>
        /// <returns>An operation result.</returns>
        Task<OperationResult> InsertCollectionElementAsync(string collectionName, int id, IDictionary<string, string> data);

        /// <summary>
        /// Deletes an existed collection element from the collection with the specified <paramref name="collectionName"/>.
        /// </summary>
        /// <param name="collectionName">A collection name.</param>
        /// <param name="id">A unique identifier of a collection element to delete from the collection.</param>
        /// <returns>An operation result.</returns>
        Task<OperationResult> DeleteCollectionElementAsync(string collectionName, int id);

        /// <summary>
        /// Updates an existed collection element in the collection with the specified <paramref name="collectionName"/>.
        /// </summary>
        /// <param name="collectionName">A collection name.</param>
        /// <param name="id">A unique identifier of a collection element to update in the collection.</param>
        /// <param name="data">A collection element data.</param>
        /// <returns>An operation result.</returns>
        Task<OperationResult> UpdateCollectionElementAsync(string collectionName, int id, IDictionary<string, string> data);
    }
}
