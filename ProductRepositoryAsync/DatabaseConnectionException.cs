using System.Runtime.Serialization;

namespace ProductRepositoryAsync
{
    /// <summary>
    /// The exception is thrown when a connection to a database fails.
    /// </summary>
    [Serializable]
    public class DatabaseConnectionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseConnectionException"/> class.
        /// </summary>
        public DatabaseConnectionException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseConnectionException"/> class with a specified error message.
        /// </summary>
        public DatabaseConnectionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseConnectionException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        public DatabaseConnectionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseConnectionException"/> class with serialized data.
        /// </summary>
        protected DatabaseConnectionException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
