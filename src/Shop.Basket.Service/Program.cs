using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Shop.Basket.Service.Repository;
using Shop.Common.Identity;
using Shop.Common.MassTransit;
using Shop.Common.MongoDB;

namespace Shop.Basket.Service
{
    public class Program
    {
        private const string AllowedOriginSetting = "AllowedOrigin";
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddMongo();
            builder.Services.AddSingleton<IBasketRepository>(serviceProvider =>
            {
                var database = serviceProvider.GetService<IMongoDatabase>();
                return new BasketRepository(database!, "BasketItems");
            }).AddMassTransitWithRabbitMq()
            .AddJwtBearerAuthentication();

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Shop.Basket.Service", Version="v1" });
            });

            var app = builder.Build();

            if(app.Environment.IsDevelopment())
            {
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
