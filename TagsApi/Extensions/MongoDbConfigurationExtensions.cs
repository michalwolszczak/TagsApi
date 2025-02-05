using MongoDB.Driver;
using TagsApi.Common;
using TagsApi.Dtos;

namespace TagsApi.Extensions
{
    public static class MongoDbConfigurationExtensions
    {
        public static void ConfigureMongoDb(this IServiceCollection services, TagDatabaseSettings databaseSettings)
        {
            var mongoDbClient = new MongoClient(databaseSettings.ConnectionString);
            var mongoDatabase = mongoDbClient.GetDatabase(databaseSettings.DatabaseName);

            services.AddSingleton(mongoDatabase.GetCollection<TagDto>(databaseSettings.TagsCollectionName));
        }
    }
}
