using MongoDB.Driver;
using Shop.Basket.Service.Entities;
using Shop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Shop.Basket.Service.Repository
{
    public class BasketRepository : IBasketRepository
    //<T> : IRepository<T> where T : IEntity
    {

        private readonly IMongoCollection<Entities.Basket> dbCollection;
        //private readonly FilterDefinitionBuilder<T> filterDefinitionBuilder = Builders<T>.Filter;

        public BasketRepository(IMongoDatabase database, string collectionName)
        {
            dbCollection = database.GetCollection<Entities.Basket>(collectionName);
        }
        /*
                public async Task<IReadOnlyCollection<T>> GetItemsAsync()
                {
                    return await dbCollection.Find(filterDefinitionBuilder.Empty).ToListAsync();
                }

                public async Task<IReadOnlyCollection<T>> GetItemsAsync(Expression<Func<T, bool>> filter)
                {
                    return await dbCollection.Find(filter).ToListAsync();
                }

                public async Task<T> GetItemAsync(Guid id)
                {
                    FilterDefinition<T> filter = filterDefinitionBuilder.Eq(entity => entity.Id, id);
                    return await dbCollection.Find(filter).FirstOrDefaultAsync();
                }

                public async Task<T> GetItemAsync(Expression<Func<T, bool>> filter)
                {
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
        */
        public async Task<IReadOnlyCollection<BasketItem>> GetBasketItems(Guid userId)
        {
            var basket = await dbCollection.Find(c => c.UserId == userId).FirstOrDefaultAsync();
            return basket?.Items ?? new List<BasketItem>();
        }

        public async Task InsertBasketItem(Guid userId, BasketItem basketItem)
        {
            var basket = await dbCollection.Find(c => c.UserId == userId).FirstOrDefaultAsync();
            if(basket == null)
            {
                basket = new Entities.Basket
                {
                    UserId = userId,
                    Items = new List<BasketItem> { basketItem }
                };
                await dbCollection.InsertOneAsync(basket);
            }
            else
            {
                var ci = basket
                    .Items
                    .FirstOrDefault(ci => ci.CatalogItemId == basketItem.CatalogItemId);

                if(ci == null)
                {
                    basket.Items.Add(basketItem);
                }
                else
                {
                    ci.Quantity++;
                }

                var update = Builders<Entities.Basket>.Update
                    .Set(c => c.Items, basket.Items);
                await dbCollection.UpdateOneAsync(c => c.UserId == userId, update);
            }
        }

        public async Task UpdateBasketItem(Guid userId, BasketItem basketItem)
        {
            var basket = await dbCollection.Find(c => c.UserId == userId).FirstOrDefaultAsync();
            if(basket != null)
            {
                basket.Items.RemoveAll(ci => ci.CatalogItemId == basketItem.CatalogItemId);
                basket.Items.Add(basketItem);
                var update = Builders<Entities.Basket>.Update
                    .Set(c => c.Items, basket.Items);
                await dbCollection.UpdateOneAsync(c => c.UserId == userId, update);
            }
        }

        public async Task DeleteBasketItem(Guid userId, Guid catalogItemId)
        {
            var basket = await dbCollection.Find(c => c.UserId == userId).FirstOrDefaultAsync();
            if(basket != null)
            {
                basket.Items.RemoveAll(ci => ci.CatalogItemId == catalogItemId);
                var update = Builders<Entities.Basket>.Update
                    .Set(c => c.Items, basket.Items);
                await dbCollection.UpdateOneAsync(c => c.UserId == userId, update);
            }
        }

        public async Task UpdateCatalogItem(Guid catalogItemId, string name, decimal price)
        {
            // Update catalog item in baskets
            var baskets = await GetBaskets(catalogItemId);
            foreach(var basket in baskets)
            {
                var basketItem = basket.Items.FirstOrDefault
                               (i => i.CatalogItemId == catalogItemId);
                if(basketItem != null)
                {
                    basketItem.Name = name;
                    basketItem.Price = price;
                    var update = Builders<Entities.Basket>.Update
                        .Set(c => c.Items, basket.Items);
                    await dbCollection.UpdateOneAsync(c => c.Id == basket.Id, update);
                }
            }
        }

        public async Task DeleteCatalogItem(Guid catalogItemId)
        {
            // Delete catalog item from baskets
            var baskets = await GetBaskets(catalogItemId);
            foreach(var basket in baskets)
            {
                basket.Items.RemoveAll(i => i.CatalogItemId == catalogItemId);
                var update = Builders<Entities.Basket>.Update
                    .Set(c => c.Items, basket.Items);
                await dbCollection.UpdateOneAsync(c => c.Id == basket.Id, update);
            }
        }

        private async Task<IList<Entities.Basket>> GetBaskets(Guid catalogItemId) =>
          await dbCollection.Find(c => c.Items.Any(i => i.CatalogItemId == catalogItemId)).ToListAsync();

    }
}
