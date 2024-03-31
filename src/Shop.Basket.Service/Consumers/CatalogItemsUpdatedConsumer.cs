using MassTransit;
using Shop.Basket.Service.Repository;
using Shop.Catalog.Contracts;
using System.Threading.Tasks;

namespace Shop.Basket.Service.Consumers
{
    public class CatalogItemsUpdatedConsumer : IConsumer<CatalogItemsUpdated>
    {
        private readonly IBasketRepository repository;

        public CatalogItemsUpdatedConsumer(IBasketRepository repository)
        {
            this.repository = repository;
        }
        public async Task Consume(ConsumeContext<CatalogItemsUpdated> context)
        {
            var message = context.Message;

            await repository.UpdateCatalogItem(message);
        }
    }
}
