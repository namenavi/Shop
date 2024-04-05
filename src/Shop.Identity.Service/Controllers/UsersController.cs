using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Identity.Contracts;
using Shop.Identity.Service.Entities;
using Shop.Identity.Service.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

//using static IdentityServer4.IdentityServerConstants;
using static Shop.Identity.Service.Dtos.Dtos;
using static Shop.Identity.Service.Roles.Roles;

namespace Shop.Identity.Service.Controllers
{
    [ApiController]
    [Route("Users")]
    [Authorize(Policy = LocalApi.PolicyName, Roles = Admin)]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPublishEndpoint publishEndpoint;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserDto>> Get()
        {
            var users = userManager.Users
                                    .ToList()
                                    .Select(user => user.AsDto());
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetByIdAsync(Guid id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            if(user == null)
                return NotFound();

            return Ok(user.AsDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdateUserDto userDto)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            if(user == null)
                return NotFound();

            user.Email = userDto.Email;
            // соглашение в ASP.NET Core Identity заключается в том, что имя пользователя должно совпадать с адресом электронной почты.
            user.UserName = userDto.Email;
            user.Money = userDto.Money;

            await userManager.UpdateAsync(user);


            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            if(user == null)
                return NotFound();

            await userManager.DeleteAsync(user);

            /*
                Здесь у нас может быть два варианта>
                  (1) Создайте пользовательский контракт на удаление.
                  (2) Не удаляйте пользователя и не давайте ему 0 денег, чтобы он не мог совершать покупки на платформе.
             */

            return NoContent();
        }

    }
}
