using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;
using Shop.Common.Settings;
using System;

namespace Shop.Common.HealthChecks
{
    public static class Extensions
    {
        public static IHealthChecksBuilder AddMongoDbHealthCheck(this IHealthChecksBuilder builder)
        {
            return builder.Add(registration: new HealthCheckRegistration(
                    "MongoDb Health Check",
                    serviceProvider =>
                    {
                        IConfiguration configuration = serviceProvider.GetService<IConfiguration>();
                        var mongoDbSettings = configuration
                                                                        .GetSection(nameof(MongoDbSettings))
                                                                        .Get<MongoDbSettings>();
                        var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
                        return new MongoDbHealthChecks(mongoClient);
                    },
                    HealthStatus.Unhealthy,
                    //���� ��� ��������� �������� �����������������
                    new[] { "ready" },
                    TimeSpan.FromSeconds(5)
            ));
        }

        public static void MapPlayEconomyHealthChecks(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions()
            {
                Predicate = (check) => check.Tags.Contains("ready")
            });
            endpoints.MapHealthChecks("/health/live", new HealthCheckOptions()
            {
                //�� ���������� false, ������ ��� ��� �� ���������� ��������� ��������, � ���������� ������ ��������� ������ �� ������
                Predicate = (check) => false
            });
        }

    }
}