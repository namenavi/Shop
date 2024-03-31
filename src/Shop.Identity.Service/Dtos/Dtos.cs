using System;
using System.ComponentModel.DataAnnotations;

namespace Shop.Identity.Service.Dtos
{
    public class Dtos
    {
        public record UserDto(Guid id, string username, string Email, decimal Money, DateTimeOffset CreatedDate);
        public record UpdateUserDto([Required][EmailAddress] string Email, [Range(0, 1000000)] decimal Money);
    }
}
