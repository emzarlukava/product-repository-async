using System.Runtime.Serialization;

namespace ProductRepositoryAsync
{
    /// <summary>
    /// The exception is thrown when there is an invalid attempt to find a product.
    /// </summary>
    [Serializable]
    public class ProductNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductNotFoundException"/> class.
        /// </summary>
        public ProductNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductNotFoundException"/> class with a specified error message.
        /// </summary>
        public ProductNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductNotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        public ProductNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductNotFoundException"/> class with serialized data.
        /// </summary>
        protected ProductNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
