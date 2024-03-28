using MongoDB.Driver;
using Shop.Catalog.Service.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Catalog.Service.Repositories
{
    public class MongoRepository<T> : IRepository<T> where T : IEntity
    {

        private readonly IMongoCollection<T> dbCollection;
        private readonly FilterDefinitionBuilder<T> filterDefinitionBuilder = Builders<T>.Filter;

        public MongoRepository(IMongoDatabase database, string collectionName)
        {
            dbCollection = database.GetCollection<T>(collectionName);
        }

        public async Task<IReadOnlyCollection<T>> GetItemsAsync()
        {
            return await dbCollection.Find(filterDefinitionBuilder.Empty).ToListAsync();
        }

        public async Task<T> GetItemAsync(Guid id)
        {
            FilterDefinition<T> filter = filterDefinitionBuilder.Eq(entity => entity.Id, id);
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateItemsAsync(T item)
        {
            if(item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            await dbCollection.InsertOneAsync(item);
        }

        public async Task UpdateItemAsync(T item)
        {
            if(item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            FilterDefinition<T> filter = filterDefinitionBuilder.Eq(entity => entity.Id, item.Id);
            await dbCollection.ReplaceOneAsync(filter, item);
        }

        public async Task RemoveItemAsync(Guid id)
        {
            FilterDefinition<T> filter = filterDefinitionBuilder.Eq(entity => entity.Id, id);
            await dbCollection.DeleteOneAsync(filter);
        }
    }
}
