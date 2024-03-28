using MongoDB.Driver;
using Shop.Catalog.Service.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Catalog.Service.Repositories
{
    public class CatalogRepository : ICatalogRepository
    {
        private const string collectionName = "items";
        private readonly IMongoCollection<CatalogItem> dbCollection;
        private readonly FilterDefinitionBuilder<CatalogItem> filterDefinitionBuilder = Builders<CatalogItem>.Filter;

        public CatalogRepository(IMongoDatabase database)
        {
            //var mongoClient = new MongoClient("mongodb://localhost:27017");
            //var dataBase = mongoClient.GetDatabase("Catalog");
            dbCollection = database.GetCollection<CatalogItem>(collectionName);
        }

        public async Task<IReadOnlyCollection<CatalogItem>> GetItemsAsync()
        {
            return await dbCollection.Find(filterDefinitionBuilder.Empty).ToListAsync();
        }

        public async Task<CatalogItem> GetItemAsync(Guid id)
        {
            FilterDefinition<CatalogItem> filter = filterDefinitionBuilder.Eq(entity => entity.Id, id);
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateItemsAsync(CatalogItem item)
        {
            if(item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            await dbCollection.InsertOneAsync(item);
        }

        public async Task UpdateItemAsync(CatalogItem item)
        {
            if(item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            FilterDefinition<CatalogItem> filter = filterDefinitionBuilder.Eq(entity => entity.Id, item.Id);
            await dbCollection.ReplaceOneAsync(filter, item);
        }

        public async Task RemoveItemAsync(Guid id)
        {
            FilterDefinition<CatalogItem> filter = filterDefinitionBuilder.Eq(entity => entity.Id, id);
            await dbCollection.DeleteOneAsync(filter);
        }
    }
}
