using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Shop.Catalog.Service.Entities;
using Shop.Common.Identity;
using Shop.Common.MassTransit;
using Shop.Common.MongoDB;
using Shop.Common.Settings;

namespace Shop.Catalog.Service
{
    public class Program
    {
        private const string AllowedOriginSetting = "AllowedOrigin";
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var serviceSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

            builder.Services.AddMongo()
                .AddMongoRepository<CatalogItem>("items")
                .AddMassTransitWithRabbitMq()
                .AddJwtBearerAuthentication();

            builder.Services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Shop.Catalog.Service", Version="v1" });
            });

            var app = builder.Build();

            if(app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseCors(build =>
                {
                    build.WithOrigins(builder.Configuration[AllowedOriginSetting])
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}



