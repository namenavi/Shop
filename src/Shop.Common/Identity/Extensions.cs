using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace Shop.Common.Identity
{
    public static class Extensions
    {
        public static AuthenticationBuilder AddJwtBearerAuthentication(this IServiceCollection services)
        {
            return services
                        .ConfigureOptions<ConfigureJwtBearerOptions>()
                        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        //на самом деле нам не нужно указывать параметры внутри, потому что мы указали их выше
                        .AddJwtBearer();
        }
    }
}
