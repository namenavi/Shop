using Shop.Identity.Service.Entities;
using static Shop.Identity.Service.Dtos.Dtos;

namespace Shop.Identity.Service.Extensions
{
    public static class Extensions
    {
        public static UserDto AsDto(this ApplicationUser user)
        {
            return new UserDto(user.Id, user.UserName, user.Email, user.Money, user.CreatedOn);
        }
    }
}
