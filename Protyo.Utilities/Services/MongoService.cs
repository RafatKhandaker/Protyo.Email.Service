using MongoDB.Driver;
using Protyo.Utilities.Configuration.Contracts;
using Protyo.Utilities.Services.Contracts;
using System.Collections.Generic;

namespace Protyo.Utilities.Services
{
    public class MongoService<T> : IMongoService<T>
    {
        IConfigurationSetting _configurationSetting;
        private MongoClient Client { get; set; }
        private IMongoDatabase Database { get; set; }
        private IMongoCollection<T> Collections { get; set; }

        public MongoService(IConfigurationSetting configuration) {
            _configurationSetting = configuration;
            Client = new MongoClient( MongoClientSettings.FromUrl( new MongoUrl(_configurationSetting.appSettings["MongoSettings:Url"]) ) );
        }

        public MongoService<T> SetDatabase(string databaseName) { 
            Database = Client.GetDatabase(databaseName);
            return this; 
        }

        public MongoService<T> SetCollections(string collection) {
            Collections = Database.GetCollection<T>(collection);
            return this;
        }

        public List<T> RetrieveAll() => 
            Collections.Find<T>(_ => true).ToListAsync().Result;

        public async void InsertOne(T entity) => 
            await Collections.InsertOneAsync(entity);
        
        public List<T> Find(FilterDefinition<T> filter) =>  
            Collections.Find<T>(filter).ToListAsync().Result;

        public long Update(FilterDefinition<T> updateFilter, UpdateDefinition<T> update) => 
            Collections.UpdateOneAsync(updateFilter, update, new UpdateOptions { IsUpsert = true }).Result.ModifiedCount;
       
        public long Delete(FilterDefinition<T> filter ) => 
            Collections.DeleteOneAsync(filter).Result.DeletedCount;
    }
}
