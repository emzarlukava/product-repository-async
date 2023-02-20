[assembly: CLSCompliant(true)]

namespace ProductRepositoryAsync
{
    /// <summary>
    /// Represents a product.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Gets or sets a product identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a product name.
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// Gets or sets a product category.
        /// </summary>
        public string Category { get; set; } = default!;

        /// <summary>
        /// Gets or sets a price of a product unit.
        /// </summary>
        public decimal UnitPrice { get; set; } = default!;

        /// <summary>
        /// Gets or sets a number of units in stock.
        /// </summary>
        public int UnitsInStock { get; set; } = default!;

        /// <summary>
        /// Gets or sets a value indicating whether a product is discontinued.
        /// </summary>
        public bool Discontinued { get; set; } = default!;
    }
}
