using Shop.Basket.Service.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Basket.Service.Repository
{
    public interface IBasketRepository
    {
        Task DeleteBasketItem(Guid userId, Guid catalogItemId);
        Task DeleteCatalogItem(Guid catalogItemId);
        Task<IReadOnlyCollection<BasketItem>> GetBasketItems(Guid userId);
        Task InsertBasketItem(Guid userId, BasketItem basketItem);
        Task UpdateBasketItem(Guid userId, BasketItem basketItem);
        Task UpdateCatalogItem(Guid catalogItemId, string name, decimal price);
    }
}