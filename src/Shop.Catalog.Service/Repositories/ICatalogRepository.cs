using Shop.Catalog.Service.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Catalog.Service.Repositories
{
    public interface ICatalogRepository
    {
        Task CreateItemsAsync(CatalogItem item);
        Task<CatalogItem> GetItemAsync(Guid id);
        Task<IReadOnlyCollection<CatalogItem>> GetItemsAsync();
        Task RemoveItemAsync(Guid id);
        Task UpdateItemAsync(CatalogItem item);
    }
}
