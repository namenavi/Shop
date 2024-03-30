using Shop.Common;
using System;
using System.Collections.Generic;

namespace Shop.Basket.Service.Entities
{
    public class Basket : IEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<BasketItem> Items { get; init; }
    }
}
