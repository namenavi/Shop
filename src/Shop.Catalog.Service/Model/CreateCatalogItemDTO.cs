using System;
using System.ComponentModel.DataAnnotations;

namespace Shop.Catalog.Service.Model
{
    public record CreateCatalogItemDTO([Required] string Name, string Description, [Range(0, 100000)] decimal Price, [Required] Guid CatalogTypeId);
}
