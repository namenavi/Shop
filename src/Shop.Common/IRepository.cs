using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Shop.Common
{
    public interface IRepository<T> where T : IEntity
    {
        Task CreateItemsAsync(T item);
        Task<T> GetItemAsync(Guid id);
        Task<T> GetItemAsync(Expression<Func<T, bool>> filter);
        Task<IReadOnlyCollection<T>> GetItemsAsync();
        Task<IReadOnlyCollection<T>> GetItemsAsync(Expression<Func<T, bool>> filter);
        Task RemoveItemAsync(Guid id);
        Task UpdateItemAsync(T item);
    }
}
