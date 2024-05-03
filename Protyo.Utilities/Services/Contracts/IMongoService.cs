using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protyo.Utilities.Services.Contracts
{
    public interface IMongoService<T> 
    {
        public MongoService<T> SetDatabase(string databaseName);
        public MongoService<T> SetCollections(string collection);
        public List<T> RetrieveAll();
        public void InsertOne(T entity);
        public List<T> Find(FilterDefinition<T> filter);
        public long Update(FilterDefinition<T> updateFilter, UpdateDefinition<T> update);
        public long Delete(FilterDefinition<T> filter);
    }
}
