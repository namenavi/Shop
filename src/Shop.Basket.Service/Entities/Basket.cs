using System.Collections.Generic;

namespace Shop.Basket.Service.Entities
{
    public class Basket
    {
        public string Id { get; init; }
        public string UserId { get; init; }
        public List<BasketItem> Items { get; init; }
    }
}
