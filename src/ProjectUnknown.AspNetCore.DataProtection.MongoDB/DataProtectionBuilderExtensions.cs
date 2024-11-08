using System;
using Microsoft.AspNetCore.DataProtection;
using MongoDB.Driver;
using MongoDB.Driver.Core.Misc;

namespace ProjectUnknown.AspNetCore.DataProtection.MongoDB
{
    public static class DataProtectionBuilderExtensions
    {
        private const string DataProtectionKeysName = "DataProtection-Keys";

        /// <summary>
        /// Configures the data protection system to persist keys to specified collection in MongoDB database.
        /// </summary>
        /// <param name="builder">The builder instance to modify.</param>
        /// <param name="databaseFactory">The factory used to create <see cref="IMongoDatabase"/> instances.</param>
        /// <returns>A reference to the <see cref="IDataProtectionBuilder"/> after this operation has completed.</returns>
        public static IDataProtectionBuilder PersistKeysToMongo(this IDataProtectionBuilder builder, Func<IMongoDatabase> databaseFactory)
        {
            return PersistKeysToMongo(builder, databaseFactory, DataProtectionKeysName);
        }

        /// <summary>
        /// Configures the data protection system to persist keys to specified collection in MongoDB database.
        /// </summary>
        /// <param name="builder">The builder instance to modify.</param>
        /// <param name="databaseFactory">The factory used to create <see cref="IMongoDatabase"/> instances.</param>
        /// <param name="collectionName">The name of the collection to use.</param>
        /// <returns>A reference to the <see cref="IDataProtectionBuilder"/> after this operation has completed.</returns>
        public static IDataProtectionBuilder PersistKeysToMongo(this IDataProtectionBuilder builder, Func<IMongoDatabase> databaseFactory, string collectionName)
        {
            Ensure.IsNotNull(builder, nameof(builder));
            Ensure.IsNotNull(databaseFactory, nameof(databaseFactory));
            Ensure.IsNotNullOrEmpty(collectionName, nameof(collectionName));

            return PersistKeysToMongoInternal(builder, databaseFactory, collectionName);
        }

        /// <summary>
        /// Configures the data protection system to persist keys to specified collection in MongoDB database.
        /// </summary>
        /// <param name="builder">The builder instance to modify.</param>
        /// <param name="connectionString">Connection string to MongoDB server.</param>
        /// <param name="databaseName">The name of the database to use.</param>
        /// <returns>A reference to the <see cref="IDataProtectionBuilder"/> after this operation has completed.</returns>
        public static IDataProtectionBuilder PersistKeysToMongo(this IDataProtectionBuilder builder, string connectionString, string databaseName)
        {
            return PersistKeysToMongo(builder, connectionString, databaseName, DataProtectionKeysName);
        }

        /// <summary>
        /// Configures the data protection system to persist keys to specified collection in MongoDB database.
        /// </summary>
        /// <param name="builder">The builder instance to modify.</param>
        /// <param name="connectionString">Connection string to MongoDB server.</param>
        /// <param name="databaseName">The name of the database to use.</param>
        /// <param name="collectionName">The name of the collection to use.</param>
        /// <returns>A reference to the <see cref="IDataProtectionBuilder"/> after this operation has completed.</returns>
        public static IDataProtectionBuilder PersistKeysToMongo(this IDataProtectionBuilder builder, string connectionString, string databaseName, string collectionName)
        {
            Ensure.IsNotNull(builder, nameof(builder));
            Ensure.IsNotNullOrEmpty(connectionString, nameof(connectionString));
            Ensure.IsNotNullOrEmpty(databaseName, nameof(databaseName));
            Ensure.IsNotNullOrEmpty(collectionName, nameof(collectionName));

            var client = new MongoClient(connectionString);

            return PersistKeysToMongoInternal(builder, () => client.GetDatabase(databaseName), collectionName);
        }

        private static IDataProtectionBuilder PersistKeysToMongoInternal(IDataProtectionBuilder builder, Func<IMongoDatabase> databaseFactory, string collectionName)
        {
            builder.AddKeyManagementOptions(options =>
            {
                options.XmlRepository = new MongoXmlRepository(databaseFactory, collectionName);
            });

            return builder;
        }
    }
}
