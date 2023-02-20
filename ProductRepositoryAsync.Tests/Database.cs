using System.Collections.ObjectModel;

[assembly: CLSCompliant(false)]

namespace ProductRepositoryAsync.Tests
{
    public class Database : IDatabase
    {
        private readonly object lockObject = new object();
        private readonly Dictionary<string, Collection> database = new Dictionary<string, Collection>();

        public bool IsCollectionExistAsyncFailure { get; init; }

        public bool IsCollectionElementExistFailure { get; init; }

        public bool CreateCollectionAsyncFailure { get; init; }

        public bool GenerateIdAsyncFailure { get; init; }

        public bool GetCollectionElementAsyncFailure { get; init; }

        public bool InsertCollectionElementAsyncFailure { get; init; }

        public bool DeleteCollectionElementAsyncFailure { get; init; }

        public bool UpdateCollectionElementAsyncFailure { get; init; }

        public bool ReturnConnectionIssue { get; init; }

        public Task<OperationResult> IsCollectionExistAsync(string collectionName, out bool collectionExists)
        {
            collectionExists = false;

            if (this.IsCollectionExistAsyncFailure && this.ReturnConnectionIssue)
            {
                return Task.FromResult(OperationResult.ConnectionIssue);
            }

            if (this.IsCollectionExistAsyncFailure)
            {
                return Task.FromResult(OperationResult.Failure);
            }

            if (!IsValidCollectionName(collectionName))
            {
                return Task.FromResult(OperationResult.InvalidCollectionName);
            }

            lock (this.lockObject)
            {
                if (this.HasCollection(collectionName))
                {
                    collectionExists = true;
                }
            }

            return Task.FromResult(OperationResult.Success);
        }

        public Task<OperationResult> IsCollectionElementExistAsync(string collectionName, int id, out bool collectionElementExists)
        {
            collectionElementExists = false;

            if (this.IsCollectionElementExistFailure && this.ReturnConnectionIssue)
            {
                return Task.FromResult(OperationResult.ConnectionIssue);
            }

            if (this.IsCollectionElementExistFailure)
            {
                return Task.FromResult(OperationResult.Failure);
            }

            if (!IsValidCollectionName(collectionName))
            {
                return Task.FromResult(OperationResult.InvalidCollectionName);
            }

            lock (this.lockObject)
            {
                if (!this.HasCollection(collectionName))
                {
                    return Task.FromResult(OperationResult.Failure);
                }

                collectionElementExists = this.HasCollectionElement(collectionName, id);
            }

            return Task.FromResult(OperationResult.Success);
        }

        public Task<OperationResult> CreateCollectionAsync(string collectionName)
        {
            if (this.CreateCollectionAsyncFailure && this.ReturnConnectionIssue)
            {
                return Task.FromResult(OperationResult.ConnectionIssue);
            }

            if (this.CreateCollectionAsyncFailure)
            {
                return Task.FromResult(OperationResult.Failure);
            }

            if (!IsValidCollectionName(collectionName))
            {
                return Task.FromResult(OperationResult.InvalidCollectionName);
            }

            lock (this.lockObject)
            {
                if (this.HasCollection(collectionName))
                {
                    return Task.FromResult(OperationResult.Failure);
                }

                this.database.Add(collectionName, new Collection(collectionName));

                return Task.FromResult(OperationResult.Success);
            }
        }

        public Task<OperationResult> GenerateIdAsync(string collectionName, out int id)
        {
            id = 0;

            if (this.GenerateIdAsyncFailure && this.ReturnConnectionIssue)
            {
                return Task.FromResult(OperationResult.ConnectionIssue);
            }

            if (this.GenerateIdAsyncFailure)
            {
                return Task.FromResult(OperationResult.Failure);
            }

            if (!IsValidCollectionName(collectionName))
            {
                return Task.FromResult(OperationResult.InvalidCollectionName);
            }

            Collection? collection;

            lock (this.lockObject)
            {
                if (!this.database.TryGetValue(collectionName, out collection))
                {
                    return Task.FromResult(OperationResult.Failure);
                }

                id = collection.GenerateElementId();
            }

            return Task.FromResult(OperationResult.Success);
        }

        public Task<OperationResult> GetCollectionElementAsync(string collectionName, int id, out IDictionary<string, string> data)
        {
            data = null!;

            if (this.GetCollectionElementAsyncFailure && this.ReturnConnectionIssue)
            {
                return Task.FromResult(OperationResult.ConnectionIssue);
            }

            if (this.GetCollectionElementAsyncFailure)
            {
                return Task.FromResult(OperationResult.Failure);
            }

            if (!IsValidCollectionName(collectionName))
            {
                return Task.FromResult(OperationResult.InvalidCollectionName);
            }

            lock (this.lockObject)
            {
                if (!this.database.TryGetValue(collectionName, out Collection? collection))
                {
                    return Task.FromResult(OperationResult.Failure);
                }

                if (!collection.Elements.TryGetValue(id, out CollectionElement? collectionElement))
                {
                    return Task.FromResult(OperationResult.Failure);
                }

                data = new ReadOnlyDictionary<string, string>(collectionElement.Properties);
            }

            return Task.FromResult(OperationResult.Success);
        }

        public Task<OperationResult> InsertCollectionElementAsync(string collectionName, int id, IDictionary<string, string> data)
        {
            if (this.InsertCollectionElementAsyncFailure && this.ReturnConnectionIssue)
            {
                return Task.FromResult(OperationResult.ConnectionIssue);
            }

            if (this.InsertCollectionElementAsyncFailure)
            {
                return Task.FromResult(OperationResult.Failure);
            }

            if (!IsValidCollectionName(collectionName))
            {
                return Task.FromResult(OperationResult.InvalidCollectionName);
            }

            lock (this.lockObject)
            {
                if (!this.database.TryGetValue(collectionName, out Collection? collection))
                {
                    return Task.FromResult(OperationResult.Failure);
                }

                if (collection.Elements.ContainsKey(id))
                {
                    return Task.FromResult(OperationResult.Failure);
                }

                CollectionElement element = new CollectionElement(id);

                foreach (KeyValuePair<string, string> pair in data)
                {
                    element.Properties.Add(pair.Key, pair.Value);
                }

                collection.Elements.Add(id, element);
            }

            return Task.FromResult(OperationResult.Success);
        }

        public Task<OperationResult> DeleteCollectionElementAsync(string collectionName, int id)
        {
            if (this.DeleteCollectionElementAsyncFailure && this.ReturnConnectionIssue)
            {
                return Task.FromResult(OperationResult.ConnectionIssue);
            }
            else if (this.DeleteCollectionElementAsyncFailure)
            {
                return Task.FromResult(OperationResult.Failure);
            }

            if (!IsValidCollectionName(collectionName))
            {
                return Task.FromResult(OperationResult.InvalidCollectionName);
            }

            lock (this.lockObject)
            {
                if (!this.database.TryGetValue(collectionName, out Collection? collection))
                {
                    return Task.FromResult(OperationResult.Failure);
                }

                if (!collection.Elements.ContainsKey(id))
                {
                    return Task.FromResult(OperationResult.Failure);
                }

                _ = collection.Elements.Remove(id);
            }

            return Task.FromResult(OperationResult.Success);
        }

        public Task<OperationResult> UpdateCollectionElementAsync(string collectionName, int id, IDictionary<string, string> data)
        {
            if (this.UpdateCollectionElementAsyncFailure && this.ReturnConnectionIssue)
            {
                return Task.FromResult(OperationResult.ConnectionIssue);
            }

            if (this.UpdateCollectionElementAsyncFailure)
            {
                return Task.FromResult(OperationResult.Failure);
            }

            if (!IsValidCollectionName(collectionName))
            {
                return Task.FromResult(OperationResult.InvalidCollectionName);
            }

            lock (this.lockObject)
            {
                if (!this.database.TryGetValue(collectionName, out Collection? collection))
                {
                    return Task.FromResult(OperationResult.Failure);
                }

                if (!collection.Elements.TryGetValue(id, out CollectionElement? collectionElement))
                {
                    return Task.FromResult(OperationResult.Failure);
                }

                collectionElement.Properties.Clear();

                foreach (KeyValuePair<string, string> pair in data)
                {
                    collectionElement.Properties.Add(pair.Key, pair.Value);
                }
            }

            return Task.FromResult(OperationResult.Success);
        }

        public bool HasCollection(string collectionName)
        {
            return this.database.TryGetValue(collectionName, out _);
        }

        public bool HasCollectionElement(string collectionName, int id)
        {
            if (this.database.TryGetValue(collectionName, out Collection? collection))
            {
                return collection.Elements.TryGetValue(id, out _);
            }

            return false;
        }

        private static bool IsValidCollectionName(string collectionName)
        {
            return !string.IsNullOrWhiteSpace(collectionName) && !(
                collectionName.StartsWith(' ') ||
                collectionName.EndsWith(' ') ||
                collectionName.Length < 5);
        }

        private class Collection
        {
            private int nextId = 1;

            public Collection(string collectionName)
            {
                this.Name = collectionName;
            }

            public string Name { get; private set; }

            public IDictionary<int, CollectionElement> Elements { get; } = new Dictionary<int, CollectionElement>();

            public int GenerateElementId()
            {
                return this.nextId++;
            }
        }

        private class CollectionElement
        {
            public CollectionElement(int id)
            {
                this.Id = id;
            }

            public int Id { get; private set; }

            public Dictionary<string, string> Properties { get; } = new Dictionary<string, string>();
        }
    }
}
