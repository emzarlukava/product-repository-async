using System.Globalization;
using NUnit.Framework;

namespace ProductRepositoryAsync.Tests
{
    [TestFixture]
    public class ProductRepositoryTests
    {
        private const string ProductCollectionName = "products";
        private const string NameKey = "name";
        private const string CategoryKey = "category";
        private const string UnitPriceKey = "price";
        private const string UnitsInStockKey = "in-stock";
        private const string DiscontinuedKey = "discontinued";

        [TestCase(true, false, false)]
        [TestCase(false, true, false)]
        [TestCase(false, false, true)]
        public async Task GetProductAsync_DatabaseReturnsConnectionIssue_ThrowsConnectionException(bool isCollectionExistAsyncFailure, bool isCollectionElementExistFailure, bool getCollectionElementAsyncFailure)
        {
            const int productId = 1;

            // Arrange
            var database = new Database
            {
                IsCollectionExistAsyncFailure = isCollectionExistAsyncFailure,
                IsCollectionElementExistFailure = isCollectionElementExistFailure,
                GetCollectionElementAsyncFailure = getCollectionElementAsyncFailure,
                ReturnConnectionIssue = true,
            };

            _ = await database.CreateCollectionAsync(ProductCollectionName);
            _ = await database.InsertCollectionElementAsync(ProductCollectionName, productId, new Dictionary<string, string>());
            var productRepository = new ProductRepository(ProductCollectionName, database);

            // Assert
            _ = Assert.ThrowsAsync<DatabaseConnectionException>(async () =>
            {
                // Act
                _ = await productRepository.GetProductAsync(productId);
            });
        }

        [TestCase(true, false, false)]
        [TestCase(false, true, false)]
        [TestCase(false, false, true)]
        public async Task GetProductAsync_DatabaseReturnsFailure_ThrowsRepositoryException(bool isCollectionExistAsyncFailure, bool isCollectionElementExistFailure, bool getCollectionElementAsyncFailure)
        {
            const int productId = 1;

            // Arrange
            var database = new Database
            {
                IsCollectionExistAsyncFailure = isCollectionExistAsyncFailure,
                IsCollectionElementExistFailure = isCollectionElementExistFailure,
                GetCollectionElementAsyncFailure = getCollectionElementAsyncFailure,
            };

            _ = await database.CreateCollectionAsync(ProductCollectionName);
            _ = await database.InsertCollectionElementAsync(ProductCollectionName, productId, new Dictionary<string, string>());
            var productRepository = new ProductRepository(ProductCollectionName, database);

            // Assert
            _ = Assert.ThrowsAsync<RepositoryException>(async () =>
            {
                // Act
                _ = await productRepository.GetProductAsync(productId);
            });
        }

        [TestCase("   ")]
        public void GetProductAsync_InvalidCollectionName_ThrowsRepositoryException(string collectionName)
        {
            const int productId = 1;

            // Arrange
            var database = new Database();
            var productRepository = new ProductRepository(collectionName, database);

            // Assert
            _ = Assert.ThrowsAsync<RepositoryException>(async () =>
            {
                // Act
                _ = await productRepository.GetProductAsync(productId);
            });
        }

        [Test]
        public void GetProductAsync_CollectionIsNotExist_ThrowsCollectionNotFoundException()
        {
            // Arrange
            var database = new Database();
            var productRepository = new ProductRepository(ProductCollectionName, database);

            // Assert
            _ = Assert.ThrowsAsync<CollectionNotFoundException>(async () =>
            {
                // Act
                _ = await productRepository.GetProductAsync(default);
            });
        }

        [Test]
        public async Task GetProductAsync_ProductIsNotExist_ThrowsProductNotFoundException()
        {
            // Arrange
            var database = new Database();
            _ = await database.CreateCollectionAsync(ProductCollectionName);
            var productRepository = new ProductRepository(ProductCollectionName, database);

            // Assert
            _ = Assert.ThrowsAsync<ProductNotFoundException>(async () =>
            {
                // Act
                _ = await productRepository.GetProductAsync(1);
            });
        }

        [TestCase(1, "Chai", "Beverages", 19.34, 12, false)]
        public async Task GetProductAsync_ProductIsExist_ReturnsProduct(int productId, string productName, string category, decimal price, int inStock, bool discontinued)
        {
            // Arrange
            Database database = new Database();

            _ = await database.CreateCollectionAsync(ProductCollectionName);

            var data = new Dictionary<string, string>
            {
                { NameKey, productName },
                { CategoryKey, category },
                { UnitPriceKey, price.ToString(CultureInfo.InvariantCulture) },
                { UnitsInStockKey, inStock.ToString(CultureInfo.InvariantCulture) },
                { DiscontinuedKey, discontinued.ToString(CultureInfo.InvariantCulture) },
            };

            _ = await database.InsertCollectionElementAsync(ProductCollectionName, productId, data);

            ProductRepository productRepository = new ProductRepository(ProductCollectionName, database);

            // Act
            Product product = await productRepository.GetProductAsync(productId);

            // Assert
            Assert.That(product.Id, Is.EqualTo(productId));
            Assert.That(product.Name, Is.EqualTo(productName));
            Assert.That(product.Category, Is.EqualTo(category));
            Assert.That(product.UnitPrice, Is.EqualTo(price));
            Assert.That(product.UnitsInStock, Is.EqualTo(inStock));
            Assert.That(product.Discontinued, Is.EqualTo(discontinued));
        }

        [TestCase(true, false, false, false)]
        [TestCase(false, true, false, false)]
        [TestCase(false, false, true, false)]
        [TestCase(false, false, false, true)]
        public void AddProductAsync_DatabaseReturnsConnectionIssue_ThrowsConnectionException(bool isCollectionExistAsyncFailure, bool createCollectionAsyncFailure, bool generateIdAsyncFailure, bool insertCollectionElementAsyncFailure)
        {
            const string name = "Chai";
            const string category = "Beverages";
            const decimal unitPrice = 19.34m;
            const int unitsInStock = 12;
            const bool discontinued = true;

            // Arrange
            var database = new Database
            {
                IsCollectionExistAsyncFailure = isCollectionExistAsyncFailure,
                CreateCollectionAsyncFailure = createCollectionAsyncFailure,
                GenerateIdAsyncFailure = generateIdAsyncFailure,
                InsertCollectionElementAsyncFailure = insertCollectionElementAsyncFailure,
                ReturnConnectionIssue = true,
            };

            var productRepository = new ProductRepository(ProductCollectionName, database);

            var product = new Product
            {
                Name = name,
                Category = category,
                UnitPrice = unitPrice,
                UnitsInStock = unitsInStock,
                Discontinued = discontinued,
            };

            // Assert
            _ = Assert.ThrowsAsync<DatabaseConnectionException>(async () =>
            {
                // Act
                _ = await productRepository.AddProductAsync(product);
            });
        }

        [TestCase(true, false, false, false)]
        [TestCase(false, true, false, false)]
        [TestCase(false, false, true, false)]
        [TestCase(false, false, false, true)]
        public void AddProductAsync_DatabaseReturnsFailure_ThrowsRepositoryException(bool isCollectionExistAsyncFailure, bool createCollectionAsyncFailure, bool generateIdAsyncFailure, bool insertCollectionElementAsyncFailure)
        {
            const string name = "Chai";
            const string category = "Beverages";
            const decimal unitPrice = 19.34m;
            const int unitsInStock = 12;
            const bool discontinued = true;

            // Arrange
            var database = new Database
            {
                IsCollectionExistAsyncFailure = isCollectionExistAsyncFailure,
                CreateCollectionAsyncFailure = createCollectionAsyncFailure,
                GenerateIdAsyncFailure = generateIdAsyncFailure,
                InsertCollectionElementAsyncFailure = insertCollectionElementAsyncFailure,
            };

            var productRepository = new ProductRepository(ProductCollectionName, database);

            var product = new Product
            {
                Name = name,
                Category = category,
                UnitPrice = unitPrice,
                UnitsInStock = unitsInStock,
                Discontinued = discontinued,
            };

            // Assert
            _ = Assert.ThrowsAsync<RepositoryException>(async () =>
            {
                // Act
                _ = await productRepository.AddProductAsync(product);
            });
        }

        [TestCase("   ")]
        public void AddProductAsync_InvalidCollectionName_ThrowsRepositoryException(string collectionName)
        {
            const string name = "Chai";
            const string category = "Beverages";
            const decimal unitPrice = 19.34m;
            const int unitsInStock = 12;
            const bool discontinued = true;

            // Arrange
            var database = new Database();

            var productRepository = new ProductRepository(collectionName, database);

            var product = new Product
            {
                Name = name,
                Category = category,
                UnitPrice = unitPrice,
                UnitsInStock = unitsInStock,
                Discontinued = discontinued,
            };

            // Assert
            _ = Assert.ThrowsAsync<RepositoryException>(async () =>
            {
                // Act
                _ = await productRepository.AddProductAsync(product);
            });
        }

        [TestCase("", "Beverages", 19.34, 12, true)]
        [TestCase(" ", "Beverages", 19.34, 12, true)]
        [TestCase("     ", "Beverages", 19.34, 12, true)]
        [TestCase("Chai", "", 19.34, 12, true)]
        [TestCase("Chai", " ", 19.34, 12, true)]
        [TestCase("Chai", "     ", 19.34, 12, true)]
        [TestCase("Chai", "Beverages", -0.1, 12, true)]
        [TestCase("Chai", "Beverages", 19.34, -1, true)]
        public void AddProductAsync_InvalidData_ThrowsArgumentException(string name, string category, decimal unitPrice, int unitsInStock, bool discontinued)
        {
            // Arrange
            var database = new Database();
            var productRepository = new ProductRepository(ProductCollectionName, database);
            var product = new Product
            {
                Name = name,
                Category = category,
                UnitPrice = unitPrice,
                UnitsInStock = unitsInStock,
                Discontinued = discontinued,
            };

            // Assert
            ArgumentException? exception = Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                // Act
                _ = await productRepository.AddProductAsync(product);
            });

            Assert.That(exception, Is.Not.Null);
            Assert.That(exception!.ParamName, Is.EqualTo("product"));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task AddProductAsync_ValidData_AddsProduct(int iterations)
        {
            const string name = "Chai";
            const string category = "Beverages";
            const decimal unitPrice = 19.34m;
            const int unitsInStock = 12;
            const bool discontinued = true;

            // Arrange
            Database database = new Database();
            ProductRepository productRepository = new ProductRepository(ProductCollectionName, database);
            Product product = new Product
            {
                Name = name,
                Category = category,
                UnitPrice = unitPrice,
                UnitsInStock = unitsInStock,
                Discontinued = discontinued,
            };

            var productIds = new List<int>(iterations);

            // Act
            for (int i = 0; i < iterations; i++)
            {
                var productId = await productRepository.AddProductAsync(product);
                productIds.Add(productId);
            }

            // Assert
            Assert.That(database.HasCollection(ProductCollectionName), Is.True);

            for (int i = 0; i < iterations; i++)
            {
                int productId = productIds[i];
                Assert.That(database.HasCollectionElement(ProductCollectionName, productId), Is.True);
                _ = await database.GetCollectionElementAsync(ProductCollectionName, productId, out var data);
                Assert.That(data.Count, Is.EqualTo(5));
                Assert.That(data.ContainsKey(NameKey), Is.True);
                Assert.That(data.ContainsKey(CategoryKey), Is.True);
                Assert.That(data.ContainsKey(UnitPriceKey), Is.True);
                Assert.That(data.ContainsKey(UnitsInStockKey), Is.True);
                Assert.That(data.ContainsKey(DiscontinuedKey), Is.True);
                Assert.That(data[NameKey], Is.EqualTo(name));
                Assert.That(data[CategoryKey], Is.EqualTo(category));
                Assert.That(data[UnitPriceKey], Is.EqualTo(unitPrice.ToString(CultureInfo.InvariantCulture)));
                Assert.That(data[UnitsInStockKey], Is.EqualTo(unitsInStock.ToString(CultureInfo.InvariantCulture)));
                Assert.That(data[DiscontinuedKey], Is.EqualTo(discontinued.ToString(CultureInfo.InvariantCulture)));
            }
        }

        [TestCase(true, false, false)]
        [TestCase(false, true, false)]
        [TestCase(false, false, true)]
        public async Task RemoveProductAsync_DatabaseReturnsConnectionIssue_ThrowsConnectionException(bool isCollectionExistAsyncFailure, bool isCollectionElementExistFailure, bool deleteCollectionElementAsyncFailure)
        {
            const int productId = 1;

            // Arrange
            var database = new Database
            {
                IsCollectionExistAsyncFailure = isCollectionExistAsyncFailure,
                IsCollectionElementExistFailure = isCollectionElementExistFailure,
                DeleteCollectionElementAsyncFailure = deleteCollectionElementAsyncFailure,
                ReturnConnectionIssue = true,
            };

            _ = await database.CreateCollectionAsync(ProductCollectionName);
            _ = await database.InsertCollectionElementAsync(ProductCollectionName, productId, new Dictionary<string, string>());
            var productRepository = new ProductRepository(ProductCollectionName, database);

            // Assert
            _ = Assert.ThrowsAsync<DatabaseConnectionException>(async () =>
            {
                // Act
                await productRepository.RemoveProductAsync(productId);
            });
        }

        [TestCase(true, false, false)]
        [TestCase(false, true, false)]
        [TestCase(false, false, true)]
        public async Task RemoveProductAsync_DatabaseReturnsFailure_ThrowsRepositoryException(bool isCollectionExistAsyncFailure, bool isCollectionElementExistFailure, bool deleteCollectionElementAsyncFailure)
        {
            const int productId = 1;

            // Arrange
            var database = new Database
            {
                IsCollectionExistAsyncFailure = isCollectionExistAsyncFailure,
                IsCollectionElementExistFailure = isCollectionElementExistFailure,
                DeleteCollectionElementAsyncFailure = deleteCollectionElementAsyncFailure,
            };

            _ = await database.CreateCollectionAsync(ProductCollectionName);
            _ = await database.InsertCollectionElementAsync(ProductCollectionName, productId, new Dictionary<string, string>());
            var productRepository = new ProductRepository(ProductCollectionName, database);

            // Assert
            _ = Assert.ThrowsAsync<RepositoryException>(async () =>
            {
                // Act
                await productRepository.RemoveProductAsync(productId);
            });
        }

        [TestCase("   ")]
        public void RemoveProductAsync_InvalidCollectionName_ThrowsRepositoryException(string collectionName)
        {
            const int productId = 1;

            // Arrange
            var database = new Database();
            var productRepository = new ProductRepository(collectionName, database);

            // Assert
            _ = Assert.ThrowsAsync<RepositoryException>(async () =>
            {
                // Act
                await productRepository.RemoveProductAsync(productId);
            });
        }

        [Test]
        public void RemoveProductAsync_CollectionIsNotExist_ThrowsCollectionNotFoundException()
        {
            // Arrange
            var database = new Database();
            var productRepository = new ProductRepository(ProductCollectionName, database);

            // Assert
            _ = Assert.ThrowsAsync<CollectionNotFoundException>(async () =>
            {
                // Act
                await productRepository.RemoveProductAsync(default);
            });
        }

        [Test]
        public async Task RemoveProductAsync_ProductIsNotExist_ThrowsProductNotFoundException()
        {
            // Arrange
            var database = new Database();
            _ = await database.CreateCollectionAsync(ProductCollectionName);
            var productRepository = new ProductRepository(ProductCollectionName, database);

            // Assert
            _ = Assert.ThrowsAsync<ProductNotFoundException>(async () =>
            {
                // Act
                await productRepository.RemoveProductAsync(1);
            });
        }

        [Test]
        public async Task RemoveProductAsync_ProductIsExist_RemovesProduct()
        {
            const int productId = 1;
            const string name = "Chai";
            const string category = "Beverages";
            const decimal unitPrice = 19.34m;
            const int unitsInStock = 12;
            const bool discontinued = true;

            // Arrange
            var database = new Database();
            _ = await database.CreateCollectionAsync(ProductCollectionName);

            var data = new Dictionary<string, string>
            {
                { NameKey, name },
                { CategoryKey, category },
                { UnitPriceKey, unitPrice.ToString(CultureInfo.InvariantCulture) },
                { UnitsInStockKey, unitsInStock.ToString(CultureInfo.InvariantCulture) },
                { DiscontinuedKey, discontinued.ToString(CultureInfo.InvariantCulture) },
            };

            _ = await database.InsertCollectionElementAsync(ProductCollectionName, productId, data);
            var productRepository = new ProductRepository(ProductCollectionName, database);

            // Act
            await productRepository.RemoveProductAsync(productId);

            // Assert
            Assert.That(database.HasCollection(ProductCollectionName), Is.True);
            Assert.That(database.HasCollectionElement(ProductCollectionName, productId), Is.False);
        }

        [TestCase(true, false, false)]
        [TestCase(false, true, false)]
        [TestCase(false, false, true)]
        public async Task UpdateProductAsync_DatabaseReturnsConnectionIssue_ThrowsConnectionException(bool isCollectionExistAsyncFailure, bool isCollectionElementExistFailure, bool updateCollectionElementAsyncFailure)
        {
            const int productId = 1;
            const string newName = "1Chai1";
            const string newCategory = "2Beverages2";
            const decimal newUnitPrice = 854.294m;
            const int newUnitsInStock = 539;
            const bool newDiscontinued = false;

            // Arrange
            var database = new Database
            {
                IsCollectionExistAsyncFailure = isCollectionExistAsyncFailure,
                IsCollectionElementExistFailure = isCollectionElementExistFailure,
                UpdateCollectionElementAsyncFailure = updateCollectionElementAsyncFailure,
                ReturnConnectionIssue = true,
            };

            _ = await database.CreateCollectionAsync(ProductCollectionName);
            _ = await database.InsertCollectionElementAsync(ProductCollectionName, productId, new Dictionary<string, string>());
            var productRepository = new ProductRepository(ProductCollectionName, database);

            // Assert
            _ = Assert.ThrowsAsync<DatabaseConnectionException>(async () =>
            {
                // Act
                await productRepository.UpdateProductAsync(new Product
                {
                    Id = productId,
                    Name = newName,
                    Category = newCategory,
                    UnitPrice = newUnitPrice,
                    UnitsInStock = newUnitsInStock,
                    Discontinued = newDiscontinued,
                });
            });
        }

        [TestCase(true, false, false)]
        [TestCase(false, true, false)]
        [TestCase(false, false, true)]
        public async Task UpdateProductAsync_DatabaseReturnsFailure_ThrowsRepositoryException(bool isCollectionExistAsyncFailure, bool isCollectionElementExistFailure, bool updateCollectionElementAsyncFailure)
        {
            const int productId = 1;
            const string newName = "1Chai1";
            const string newCategory = "2Beverages2";
            const decimal newUnitPrice = 854.294m;
            const int newUnitsInStock = 539;
            const bool newDiscontinued = false;

            // Arrange
            var database = new Database
            {
                IsCollectionExistAsyncFailure = isCollectionExistAsyncFailure,
                IsCollectionElementExistFailure = isCollectionElementExistFailure,
                UpdateCollectionElementAsyncFailure = updateCollectionElementAsyncFailure,
            };

            _ = await database.CreateCollectionAsync(ProductCollectionName);
            _ = await database.InsertCollectionElementAsync(ProductCollectionName, productId, new Dictionary<string, string>());
            var productRepository = new ProductRepository(ProductCollectionName, database);

            // Assert
            _ = Assert.ThrowsAsync<RepositoryException>(async () =>
            {
                // Act
                await productRepository.UpdateProductAsync(new Product
                {
                    Id = productId,
                    Name = newName,
                    Category = newCategory,
                    UnitPrice = newUnitPrice,
                    UnitsInStock = newUnitsInStock,
                    Discontinued = newDiscontinued,
                });
            });
        }

        [TestCase("   ")]
        public void UpdateProductAsync_InvalidCollectionName_ThrowsRepositoryException(string collectionName)
        {
            const int productId = 1;
            const string newName = "1Chai1";
            const string newCategory = "2Beverages2";
            const decimal newUnitPrice = 854.294m;
            const int newUnitsInStock = 539;
            const bool newDiscontinued = false;

            // Arrange
            var database = new Database();
            var productRepository = new ProductRepository(collectionName, database);

            // Assert
            _ = Assert.ThrowsAsync<RepositoryException>(async () =>
            {
                // Act
                await productRepository.UpdateProductAsync(new Product
                {
                    Id = productId,
                    Name = newName,
                    Category = newCategory,
                    UnitPrice = newUnitPrice,
                    UnitsInStock = newUnitsInStock,
                    Discontinued = newDiscontinued,
                });
            });
        }

        [Test]
        public void UpdateProductAsync_CollectionIsNotExist_ThrowsCollectionNotFoundException()
        {
            const string newName = "1Chai1";
            const string newCategory = "2Beverages2";
            const decimal newUnitPrice = 854.294m;
            const int newUnitsInStock = 539;
            const bool newDiscontinued = false;

            // Arrange
            var database = new Database();
            var productRepository = new ProductRepository(ProductCollectionName, database);

            // Assert
            _ = Assert.ThrowsAsync<CollectionNotFoundException>(async () =>
            {
                // Act
                await productRepository.UpdateProductAsync(new Product
                {
                    Id = 1,
                    Name = newName,
                    Category = newCategory,
                    UnitPrice = newUnitPrice,
                    UnitsInStock = newUnitsInStock,
                    Discontinued = newDiscontinued,
                });
            });
        }

        [Test]
        public async Task UpdateProductAsync_ProductIsNotExist_ThrowsProductNotFoundException()
        {
            const string newName = "1Chai1";
            const string newCategory = "2Beverages2";
            const decimal newUnitPrice = 854.294m;
            const int newUnitsInStock = 539;
            const bool newDiscontinued = false;

            // Arrange
            var database = new Database();
            _ = await database.CreateCollectionAsync(ProductCollectionName);
            var productRepository = new ProductRepository(ProductCollectionName, database);

            // Assert
            _ = Assert.ThrowsAsync<ProductNotFoundException>(async () =>
            {
                // Act
                await productRepository.UpdateProductAsync(new Product
                {
                    Id = 1,
                    Name = newName,
                    Category = newCategory,
                    UnitPrice = newUnitPrice,
                    UnitsInStock = newUnitsInStock,
                    Discontinued = newDiscontinued,
                });
            });
        }

        [TestCase(1, "", "Beverages", 19.34, 12, true)]
        [TestCase(1, " ", "Beverages", 19.34, 12, true)]
        [TestCase(1, "      ", "Beverages", 19.34, 12, true)]
        [TestCase(1, "Chai", "", 19.34, 12, true)]
        [TestCase(1, "Chai", " ", 19.34, 12, true)]
        [TestCase(1, "Chai", "     ", 19.34, 12, true)]
        [TestCase(1, "Chai", "Beverages", -0.1, 12, true)]
        [TestCase(1, "Chai", "Beverages", 19.34, -1, true)]
        public async Task UpdateProductAsync_InvalidData_ThrowsArgumentException(int productId, string newName, string newCategory, decimal newUnitPrice, int newUnitsInStock, bool newDiscontinued)
        {
            // Arrange
            var database = new Database();
            _ = await database.CreateCollectionAsync(ProductCollectionName);
            _ = await database.InsertCollectionElementAsync(ProductCollectionName, productId, new Dictionary<string, string>());
            var productRepository = new ProductRepository(ProductCollectionName, database);

            // Assert
            ArgumentException? exception = Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                // Act
                await productRepository.UpdateProductAsync(new Product
                {
                    Id = productId,
                    Name = newName,
                    Category = newCategory,
                    UnitPrice = newUnitPrice,
                    UnitsInStock = newUnitsInStock,
                    Discontinued = newDiscontinued,
                });
            });

            Assert.That(exception, Is.Not.Null);
            Assert.That(exception!.ParamName, Is.EqualTo("product"));
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task UpdateProductAsync_ValidData_UpdatesProduct(int iterations)
        {
            const int productId = 1;
            const string oldName = "Chai";
            const string oldCategory = "Beverages";
            const decimal oldUnitPrice = 19.34m;
            const int oldUnitsInStock = 12;
            const bool oldDiscontinued = true;
            const string newName = "1Chai1";
            const string newCategory = "2Beverages2";
            const decimal newUnitPrice = 854.294m;
            const int newUnitsInStock = 539;
            const bool newDiscontinued = false;

            // Arrange
            var database = new Database();
            _ = await database.CreateCollectionAsync(ProductCollectionName);

            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { NameKey, oldName },
                { CategoryKey, oldCategory },
                { UnitPriceKey, oldUnitPrice.ToString(CultureInfo.InvariantCulture) },
                { UnitsInStockKey, oldUnitsInStock.ToString(CultureInfo.InvariantCulture) },
                { DiscontinuedKey, oldDiscontinued.ToString(CultureInfo.InvariantCulture) },
            };

            _ = await database.InsertCollectionElementAsync(ProductCollectionName, productId, data);
            var productRepository = new ProductRepository(ProductCollectionName, database);

            // Act
            for (int i = 0; i < iterations; i++)
            {
                await productRepository.UpdateProductAsync(new Product
                {
                    Id = productId,
                    Name = newName,
                    Category = newCategory,
                    UnitPrice = newUnitPrice,
                    UnitsInStock = newUnitsInStock,
                    Discontinued = newDiscontinued,
                });
            }

            // Assert
            Assert.That(database.HasCollection(ProductCollectionName), Is.True);
            Assert.That(database.HasCollectionElement(ProductCollectionName, productId), Is.True);
            _ = await database.GetCollectionElementAsync(ProductCollectionName, productId, out data);
            Assert.That(data.Count, Is.EqualTo(5));
            Assert.That(data.ContainsKey(NameKey), Is.True);
            Assert.That(data.ContainsKey(CategoryKey), Is.True);
            Assert.That(data.ContainsKey(UnitPriceKey), Is.True);
            Assert.That(data.ContainsKey(UnitsInStockKey), Is.True);
            Assert.That(data.ContainsKey(DiscontinuedKey), Is.True);
            Assert.That(data[NameKey], Is.EqualTo(newName));
            Assert.That(data[CategoryKey], Is.EqualTo(newCategory));
            Assert.That(data[UnitPriceKey], Is.EqualTo(newUnitPrice.ToString(CultureInfo.InvariantCulture)));
            Assert.That(data[UnitsInStockKey], Is.EqualTo(newUnitsInStock.ToString(CultureInfo.InvariantCulture)));
            Assert.That(data[DiscontinuedKey], Is.EqualTo(newDiscontinued.ToString(CultureInfo.InvariantCulture)));
        }
    }
}
