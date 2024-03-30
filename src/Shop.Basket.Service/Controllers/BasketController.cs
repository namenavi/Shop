using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Basket.Service.Entities;
using Shop.Basket.Service.Repository;
using Shop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Shop.Basket.Service.Controllers
{
    [Route("items")]
    [ApiController]
    public class BasketController : Controller
    {
        private readonly IBasketRepository itemsRepository;

        public BasketController(IBasketRepository itemsRepository)
        {
            this.itemsRepository = itemsRepository;
        }

        public async Task<ActionResult<IEnumerable<BasketItem>>> GetAsync(Guid userId)
        {
            if(userId == Guid.Empty)
            {
                return BadRequest();
            }
            var items = await itemsRepository.GetBasketItems(userId);

            return Ok(items);
        }

        public async Task<ActionResult> PostAsync(Guid userId, BasketItem basketItem)
        {
            await itemsRepository.InsertBasketItem(userId, basketItem);
            return Ok();
        }
    }
}
