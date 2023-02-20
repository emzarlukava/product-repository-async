namespace ProductRepositoryAsync
{
    /// <summary>
    /// Represents a product storage service and provides a set of methods for managing the list of products.
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Gets a product from the product repository.
        /// </summary>
        /// <param name="productId">A product identifier.</param>
        /// <returns>A <see cref="Product"/>.</returns>
        /// <exception cref="DatabaseConnectionException">database connection is lost.</exception>
        /// <exception cref="CollectionNotFoundException">collection is not found.</exception>
        /// <exception cref="ProductNotFoundException">product is not found.</exception>
        /// <exception cref="RepositoryException">a database error occurred.</exception>
        Product GetProduct(int productId);

        /// <summary>
        /// Adds a product to the product repository.
        /// </summary>
        /// <param name="product">A <see cref="Product"/>.</param>
        /// <returns>A product identifier.</returns>
        /// <exception cref="DatabaseConnectionException">database connection is lost.</exception>
        /// <exception cref="RepositoryException">a database error occurred.</exception>
        int AddProduct(Product product);

        /// <summary>
        /// Removes a product from the product repository.
        /// </summary>
        /// <param name="productId">A product identifier.</param>
        /// <exception cref="DatabaseConnectionException">database connection is lost.</exception>
        /// <exception cref="CollectionNotFoundException">collection is not found.</exception>
        /// <exception cref="ProductNotFoundException">product is not found.</exception>
        /// <exception cref="RepositoryException">a database error occurred.</exception>
        void RemoveProduct(int productId);

        /// <summary>
        /// Updates a product in the product repository.
        /// </summary>
        /// <param name="product">A <see cref="Product"/>.</param>
        /// <exception cref="DatabaseConnectionException">database connection is lost.</exception>
        /// <exception cref="CollectionNotFoundException">collection is not found.</exception>
        /// <exception cref="ProductNotFoundException">product is not found.</exception>
        /// <exception cref="RepositoryException">a database error occurred.</exception>
        void UpdateProduct(Product product);
    }
}
