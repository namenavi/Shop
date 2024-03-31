using System;

namespace Shop.Catalog.Contracts
{
    public record CatalogItemsCreated(Guid ItemId, string Name, decimal Price);//нужно ли?
    public record CatalogItemsUpdated(Guid ItemId, string Name, decimal Price);
    public record CatalogItemsDeleted(Guid ItemId);
}
