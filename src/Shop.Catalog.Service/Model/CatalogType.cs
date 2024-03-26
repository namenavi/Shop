using System;
using System.ComponentModel.DataAnnotations;

namespace Shop.Catalog.Service.Model
{
    public class CatalogType
    {
        public Guid Id { get; set; }

        [Required]
        public string Type { get; set; }
    }
}