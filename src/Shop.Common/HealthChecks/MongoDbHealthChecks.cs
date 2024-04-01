using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shop.Common.HealthChecks
{
    //пользовательские проверки работоспособности должны реализовывать интерфейс IHealth Check.
    public class MongoDbHealthChecks : IHealthCheck
    {

        private readonly MongoClient _client;

        public MongoDbHealthChecks(MongoClient client)
        {
            _client = client;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                await _client.ListDatabaseNamesAsync(cancellationToken);
                return HealthCheckResult.Healthy();
            }
            catch(Exception exception)
            {
                return HealthCheckResult.Unhealthy(exception: exception);
            }
        }
    }
}