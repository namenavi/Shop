﻿using Shop.Common;
using System;

namespace Shop.Basket.Service.Entities
{
    public class CatalogItem : IEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
    }
}
