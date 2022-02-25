using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using INachalnicUtilities.DataAccess.Mongo;

namespace INachalnicUtilities.Mongo
{
    public static class INachalnickMongo
    {
        // http://mongodb.github.io/mongo-csharp-driver/2.8/reference/driver/error_handling/
        private static readonly Retrier Retrier = new Retrier(3, TimeSpan.FromSeconds(30), e =>
            e is TimeoutException || e is MongoConnectionException);

        public static IMongoCollection<TDocument> GetCollection<TDocument>(INachalnickMongoDb db, string collectionName)
            => GetCollectionAsync<TDocument>(db, collectionName).Result;

        public static Task<IMongoCollection<TDocument>> GetCollectionAsync<TDocument>(INachalnickMongoDb db, string collectionName)
        {
            return Retrier.RetryAsync(async () =>
            {
                var database = db.Database;
                if (!await IsCollectionExists(database, collectionName))
                {
                    throw new ArgumentException($"Collection does not exists: '{collectionName}'", nameof(collectionName));
                }

                var collection = database.GetCollection<TDocument>(collectionName);
                return collection;
            });
        }

        public static Task<bool> IsCollectionExistsAsync(INachalnickMongoDb database, string collectionName)
            => IsCollectionExists(database.Database, collectionName);

        private static async Task<bool> IsCollectionExists(IMongoDatabase database, string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);
            var collectionCursor = await database.ListCollectionsAsync(new ListCollectionsOptions { Filter = filter });
            return await collectionCursor.AnyAsync();
        }
    }

    public class INachalnickMongoDb
    {
        public string Name { get; }
        private IMongoClient Client { get; }
        public IMongoDatabase Database => Client.GetDatabase(Name);

        private INachalnickMongoDb(string name, string connectionString)
        {
            Name = name;
            var conventionPack = new ConventionPack { new CamelCaseElementNameConvention(), new IgnoreExtraElementsConvention(true) };
            ConventionRegistry.Register("camelCase", conventionPack, t => true);
            var settings = MongoClientSettings.FromConnectionString(connectionString);
            settings.ClusterConfigurator = builder => builder.Subscribe(new SafeMongoDbEventSubscriber());
            Client = new MongoClient(settings);
        }
        private static Lazy<INachalnickMongoDb> _lazyTestData = new Lazy<INachalnickMongoDb>(() => new INachalnickMongoDb("mylib", "mongodb://localhost:27017/"));
        public static INachalnickMongoDb TestData => _lazyTestData.Value;
    }
}