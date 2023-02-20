using System.Runtime.Serialization;

namespace ProductRepositoryAsync
{
    /// <summary>
    /// The exception is thrown when there is an invalid attempt to find a collection.
    /// </summary>
    [Serializable]
    public class CollectionNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionNotFoundException"/> class.
        /// </summary>
        public CollectionNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionNotFoundException"/> class with a specified error message.
        /// </summary>
        public CollectionNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionNotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        public CollectionNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionNotFoundException"/> class with serialized data.
        /// </summary>
        protected CollectionNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
