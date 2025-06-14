﻿//using System;
//using Audit.Core;
//using Audit.Core.ConfigurationApi;
//using Audit.MongoDB.ConfigurationApi;
//using Newtonsoft.Json;

//namespace Library.MongoDb
//{
//    public static class MongoConfiguratorExtensions
//    {
//        /// <summary>
//        /// Store the events in a MongoDB database.
//        /// </summary>
//        /// <param name="configurator">The Audit.NET Configurator</param>
//        /// <param name="connectionString">The mongo DB connection string.</param>
//        /// <param name="database">The mongo DB database name.</param>
//        /// <param name="collection">The mongo DB collection name.</param>
//        /// <param name="jsonSerializerSettings">The custom JsonSerializerSettings.</param>
//        /// <param name="serializeAsBson">Specifies whether the target object and extra fields should be serialized as Bson. Default is Json.</param>
//        public static ICreationPolicyConfigurator UseMongoDb(this IConfigurator configurator, string connectionString = "mongodb://localhost:27017",
//            string database = "Audit", string collection = "Event", JsonSerializerSettings jsonSerializerSettings = null, bool serializeAsBson = false)
//        {
//            Configuration.DataProvider = new Audit.MongoDB.Providers.MongoDataProvider()
//            {
//                ConnectionString = connectionString,
//                Collection = collection,
//                Database = database,
//                SerializeAsBson = serializeAsBson
//            };
//            return new CreationPolicyConfigurator();
//        }
//        /// <summary>
//        /// Store the events in a MongoDB database.
//        /// </summary>
//        /// <param name="configurator">The Audit.NET Configurator</param>
//        /// <param name="config">The mongo DB provider configuration.</param>
//        public static ICreationPolicyConfigurator UseMongoDb(this IConfigurator configurator, Action<IMongoProviderConfigurator> config)
//        {
//            var mongoConfig = new MongoProviderConfigurator();
//            config.Invoke(mongoConfig);
//            return UseMongoDb(configurator, mongoConfig._connectionString, mongoConfig._database,
//                mongoConfig._collection, mongoConfig._jsonSerializerSettings,
//                mongoConfig._serializeAsBson);
//        }
//    }
//}