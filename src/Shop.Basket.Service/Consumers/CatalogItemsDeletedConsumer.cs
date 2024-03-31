using MassTransit;
using Shop.Basket.Service.Repository;
using Shop.Catalog.Contracts;
using System.Threading.Tasks;

namespace Shop.Basket.Service.Consumers
{
    public class CatalogItemsDeletedConsumer : IConsumer<CatalogItemsDeleted>
    {
        private readonly IBasketRepository repository;

        public CatalogItemsDeletedConsumer(IBasketRepository repository)
        {
            this.repository = repository;
        }
        public async Task Consume(ConsumeContext<CatalogItemsDeleted> context)
        {
            var message = context.Message;

            await repository.DeleteCatalogItem(message);
        }
    }
}