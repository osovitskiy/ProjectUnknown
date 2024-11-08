using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.AspNetCore.DataProtection.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Core.Misc;

namespace ProjectUnknown.AspNetCore.DataProtection.MongoDB
{
    /// <summary>
    /// An <see cref="IXmlRepository"/> which is backed by MongoDB.
    /// </summary>
    public class MongoXmlRepository : IXmlRepository
    {
        private readonly Func<IMongoDatabase> _databaseFactory;
        private readonly string _collectionName;

        /// <summary>
        /// Creates a new instance of the <see cref="MongoXmlRepository"/>.
        /// </summary>
        /// <param name="databaseFactory">The factory used to create <see cref="IMongoDatabase"/> instances.</param>
        /// <param name="collectionName">The name of the collection to use.</param>
        public MongoXmlRepository(Func<IMongoDatabase> databaseFactory, string collectionName)
        {
            Ensure.IsNotNull(databaseFactory, nameof(databaseFactory));
            Ensure.IsNotNullOrEmpty(collectionName, nameof(collectionName));

            _databaseFactory = databaseFactory;
            _collectionName = collectionName;
        }

        public IReadOnlyCollection<XElement> GetAllElements()
        {
            return GetAllElementsCore().ToList().AsReadOnly();
        }

        public void StoreElement(XElement element, string friendlyName)
        {
            var collection = GetCollection();
            var document = new MongoXmlDocument()
            {
                Xml = element.ToString(SaveOptions.DisableFormatting)
            };

            collection.InsertOne(document);
        }

        private IMongoCollection<MongoXmlDocument> GetCollection()
        {
            var database = _databaseFactory();

            if (database == null)
            {
                throw new InvalidOperationException("The IMongoDatabase factory method returned null.");
            }

            return database.GetCollection<MongoXmlDocument>(_collectionName);
        }

        private IEnumerable<XElement> GetAllElementsCore()
        {
            var collection = GetCollection();
            var cursor = collection.FindSync(FilterDefinition<MongoXmlDocument>.Empty);

            while (cursor.MoveNext())
            {
                foreach (var item in cursor.Current)
                {
                    yield return XElement.Parse(item.Xml);
                }
            }
        }
    }
}
