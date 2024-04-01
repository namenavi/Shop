using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Basket.Service.Entities;
using Shop.Basket.Service.Repository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Shop.Basket.Service.Controllers
{
    [Route("items")]
    [ApiController]
    //[Authorize(AdminRole)]
    public class BasketController : Controller
    {
        private const string AdminRole = "Admin";

        private readonly IBasketRepository itemsRepository;

        public BasketController(IBasketRepository itemsRepository)
        {
            this.itemsRepository = itemsRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<BasketItem>>> GetAsync(Guid userId)
        {
            if(userId == Guid.Empty)
            {
                return BadRequest();
            }

            var currentUserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if(Guid.Parse(currentUserId) != userId)
            {
                if(!User.IsInRole(AdminRole))
                {
                    return Forbid();
                }
            }

            var items = await itemsRepository.GetBasketItems(userId);

            return Ok(items);
        }

        [HttpPost]
        [Authorize(Roles = AdminRole)]
        public async Task<ActionResult> PostAsync(Guid userId, BasketItem basketItem)
        {
            await itemsRepository.InsertBasketItem(userId, basketItem);
            return Ok();
        }
    }
}
