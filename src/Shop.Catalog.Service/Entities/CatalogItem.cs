using Shop.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace Shop.Catalog.Service.Entities
{
    public class CatalogItem : IEntity
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public Guid CatalogTypeId { get; set; }

        //ToDo
        //public CatalogType CatalogType { get; set; }
    }
}
