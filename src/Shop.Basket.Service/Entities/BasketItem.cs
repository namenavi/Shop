﻿namespace Shop.Basket.Service.Entities
{
    public class BasketItem
    {
        public string CatalogItemId { get; init; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
