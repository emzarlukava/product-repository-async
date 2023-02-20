namespace ProductRepositoryAsync
{
    /// <summary>
    /// Represents a product storage service and provides a set of methods for managing the list of products.
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private readonly string productCollectionName;
        private readonly IDatabase database;

        public ProductRepository(string productCollectionName, IDatabase database)
        {
            this.productCollectionName = productCollectionName;
            this.database = database;
        }

        public int AddProduct(Product product)
        {
            // TODO Implement the method to add a product to the repository.
            throw new NotImplementedException();
        }

        public Product GetProduct(int productId)
        {
            // TODO Implement the method to get a product from the repository.
            throw new NotImplementedException();
        }

        public void RemoveProduct(int productId)
        {
            // TODO Implement the method to remove a product from the repository.
            throw new NotImplementedException();
        }

        public void UpdateProduct(Product product)
        {
            // TODO Implement the method to update a product int the repository.
            throw new NotImplementedException();
        }
    }
}
