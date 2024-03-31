using System;
using System.ComponentModel.DataAnnotations;

namespace Shop.Catalog.Service.Model
{
    public record UpdateCatalogItemDTO([Required] string Name, string Description, [Range(0, 100000)] decimal Price); //[Range(0,100000)] найти замену
}
