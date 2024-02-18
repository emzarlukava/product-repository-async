#pragma warning disable
using System.Globalization;

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

        public async Task<int> AddProductAsync(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name) || string.IsNullOrWhiteSpace(product.Category))
            {
                throw new ArgumentException("Name and Category cannot be empty or contain whitespace only.");
            }

            if (product.UnitPrice < 0)
            {
                throw new ArgumentException("UnitPrice cannot be negative.");
            }

            if (product.UnitsInStock < 0)
            {
                throw new ArgumentException("UnitsInStock cannot be negative.");
            }

            OperationResult collectionResult = await database.IsCollectionExistAsync(productCollectionName, out bool collectionExists);

            if (collectionResult != OperationResult.Success)
            {
                if (collectionResult == OperationResult.ConnectionIssue)
                {
                    throw new DatabaseConnectionException();
                }
                else
                {
                    throw new RepositoryException();
                }
            }

            if (!collectionExists)
            {
                OperationResult createResult = await database.CreateCollectionAsync(productCollectionName);

                if (createResult != OperationResult.Success)
                {
                    if (createResult == OperationResult.ConnectionIssue)
                    {
                        throw new DatabaseConnectionException();
                    }
                    else
                    {
                        throw new RepositoryException();
                    }
                }
            }

            int productId = (int)await database.GenerateIdAsync(this.productCollectionName, out productId);

            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "Id", productId.ToString() },
                { "Name", product.Name },
                { "Category", product.Category },
                { "UnitPrice", product.UnitPrice.ToString() },
                { "UnitsInStock", product.UnitsInStock.ToString() },
                { "Discontinued", product.Discontinued.ToString() }
            };

            OperationResult insertResult = await database.InsertCollectionElementAsync(productCollectionName, productId, data);

            if (insertResult != OperationResult.Success)
            {
                if (insertResult == OperationResult.ConnectionIssue)
                {
                    throw new DatabaseConnectionException();
                }
                else
                {
                    throw new RepositoryException();
                }
            }

            return productId;
        }

        public async Task<Product> GetProductAsync(int productId)
        {
            OperationResult result = await this.database.IsCollectionExistAsync(this.productCollectionName, out bool collectionExists);

            if (result == OperationResult.ConnectionIssue)
            {
                throw new DatabaseConnectionException();
            }
            else if (result != OperationResult.Success)
            {
                throw new RepositoryException();
            }

            if (!collectionExists)
            {
                throw new CollectionNotFoundException();
            }

            result = await this.database.IsCollectionElementExistAsync(this.productCollectionName, productId, out bool collectionElementExists);

            if (result == OperationResult.ConnectionIssue)
            {
                throw new DatabaseConnectionException();
            }
            else if (result != OperationResult.Success)
            {
                throw new RepositoryException();
            }

            result = await this.database.GetCollectionElementAsync(this.productCollectionName, productId, out IDictionary<string, string> data);

            if (result == OperationResult.ConnectionIssue)
            {
                throw new DatabaseConnectionException();
            }
            else if (result != OperationResult.Success)
            {
                throw new RepositoryException();
            }

            if (!collectionElementExists)
            {
                throw new ProductNotFoundException();
            }

            return new Product
            {
                Id = productId,
                Name = data["name"],
                Category = data["category"],
                UnitPrice = decimal.Parse(data["price"], CultureInfo.InvariantCulture),
                UnitsInStock = int.Parse(data["in-stock"], CultureInfo.InvariantCulture),
                Discontinued = bool.Parse(data["discontinued"]),
            };

        }

        public async Task RemoveProductAsync(int productId)
        {
            OperationResult collectionResult = await database.IsCollectionExistAsync(productCollectionName, out bool collectionExists);

            if (collectionResult != OperationResult.Success)
            {
                if (collectionResult == OperationResult.ConnectionIssue)
                {
                    throw new DatabaseConnectionException();
                }
                else
                {
                    throw new RepositoryException();
                }
            }

            if (!collectionExists)
            {
                throw new CollectionNotFoundException();
            }

            OperationResult productResult = await database.IsCollectionElementExistAsync(productCollectionName, productId, out bool productExists);

            if (productResult != OperationResult.Success)
            {
                if (productResult == OperationResult.ConnectionIssue)
                {
                    throw new DatabaseConnectionException();
                }
                else
                {
                    throw new RepositoryException();
                }
            }

            if (!productExists)
            {
                throw new ProductNotFoundException();
            }

            OperationResult deleteResult = await database.DeleteCollectionElementAsync(productCollectionName, productId);

            if (deleteResult != OperationResult.Success)
            {
                if (deleteResult == OperationResult.ConnectionIssue)
                {
                    throw new DatabaseConnectionException();
                }
                else
                {
                    throw new RepositoryException();
                }
            }
        }

        public async Task UpdateProductAsync(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name) || string.IsNullOrWhiteSpace(product.Category))
            {
                throw new ArgumentException("Name and Category cannot be empty or contain whitespace only.");
            }

            if (product.UnitPrice < 0)
            {
                throw new ArgumentException("UnitPrice cannot be negative.");
            }

            if (product.UnitsInStock < 0)
            {
                throw new ArgumentException("UnitsInStock cannot be negative.");
            }

            OperationResult collectionResult = await database.IsCollectionExistAsync(productCollectionName, out bool collectionExists);

            if (collectionResult != OperationResult.Success)
            {
                if (collectionResult == OperationResult.ConnectionIssue)
                {
                    throw new DatabaseConnectionException();
                }
                else
                {
                    throw new RepositoryException();
                }
            }

            if (!collectionExists)
            {
                throw new CollectionNotFoundException();
            }

            OperationResult productResult = await database.IsCollectionElementExistAsync(productCollectionName, product.Id, out bool productExists);

            if (productResult != OperationResult.Success)
            {
                if (productResult == OperationResult.ConnectionIssue)
                {
                    throw new DatabaseConnectionException();
                }
                else
                {
                    throw new RepositoryException();
                }
            }

            if (!productExists)
            {
                throw new ProductNotFoundException();
            }

            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "Id", product.Id.ToString() },
                { "Name", product.Name },
                { "Category", product.Category },
                { "UnitPrice", product.UnitPrice.ToString() },
                { "UnitsInStock", product.UnitsInStock.ToString() },
                { "Discontinued", product.Discontinued.ToString() }
            };

            OperationResult updateResult = await database.UpdateCollectionElementAsync(productCollectionName, product.Id, data);

            if (updateResult != OperationResult.Success)
            {
                if (updateResult == OperationResult.ConnectionIssue)
                {
                    throw new DatabaseConnectionException();
                }
                else
                {
                    throw new RepositoryException();
                }
            }
        }
    }
}
