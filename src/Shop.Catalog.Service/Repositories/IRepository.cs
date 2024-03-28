using Shop.Catalog.Service.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Catalog.Service.Repositories
{
    public interface IRepository<T> where T : IEntity
    {
        Task CreateItemsAsync(T item);
        Task<T> GetItemAsync(Guid id);
        Task<IReadOnlyCollection<T>> GetItemsAsync();
        Task RemoveItemAsync(Guid id);
        Task UpdateItemAsync(T item);
    }
}
