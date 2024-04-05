using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Catalog.Contracts;
using Shop.Catalog.Service.Entities;
using Shop.Catalog.Service.Model;
using Shop.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Shop.Catalog.Service
{
    [Route("items")]
    [ApiController]

    public class CatalogController : ControllerBase
    {
        private const string AdminRole = "Admin";
        private readonly IRepository<CatalogItem> catalogRepository;
        private readonly IPublishEndpoint publishEndpoint;

        public CatalogController(IRepository<CatalogItem> catalogRepository, IPublishEndpoint publishEndpoint)
        {
            this.catalogRepository = catalogRepository;
            this.publishEndpoint = publishEndpoint;
        }

        // GET: /items/
        [HttpGet]
        public async Task<IEnumerable<CatalogItem>> GetAsync()
        {
            var items = await catalogRepository.GetItemsAsync();
            return items;
        }

        // GET /items/5546
        [HttpGet("{id}")]
        public async Task<ActionResult<CatalogItem>> GetByIdAsync(Guid id)
        {
            var item = await catalogRepository.GetItemAsync(id);
            if(item == null)
            {
                return NotFound();
            }
            return item!;
        }

        // POST /items/
        [HttpPost]
        [Authorize(Roles = AdminRole)]
        public async Task<ActionResult<CatalogItem>> PostAsync(CreateCatalogItemDTO createItemDTO)
        {
            var item = new CatalogItem()
            {
                Id = Guid.NewGuid(),
                Name = createItemDTO.Name,
                Description = createItemDTO.Description,
                Price = createItemDTO.Price,
                //CatalogTypeId = createItemDTO.CatalogTypeId
            };

            await catalogRepository.CreateItemsAsync(item);

            // await publishEndpoint.Publish(new CatalogItemsCreated(item.Id, item.Name, item.Price));

            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
        }

        // PUT /items/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> PutAsync(Guid id, UpdateCatalogItemDTO updateItemDTO)
        {
            var existingItem = await catalogRepository.GetItemAsync(id);

            if(existingItem == null)
            {
                var item = new CatalogItem
                {
                    Id= id,
                    Name = updateItemDTO.Name,
                    Description = updateItemDTO.Description,
                    Price = updateItemDTO.Price,
                    //CatalogTypeId = updateItemDTO.CatalogTypeId
                };

                await catalogRepository.CreateItemsAsync(item);
                return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
            }

            existingItem.Price = updateItemDTO.Price;
            existingItem.Name = updateItemDTO.Name;
            existingItem.Description = updateItemDTO.Description;
            //existingItem.CatalogTypeId = updateItemDTO.CatalogTypeId;

            await publishEndpoint.Publish(new CatalogItemsUpdated(existingItem.Id, existingItem.Name, existingItem.Price));

            await catalogRepository.UpdateItemAsync(existingItem);

            return NoContent();
        }

        // DELETE /items/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var existingItem = await catalogRepository.GetItemAsync(id);

            if(existingItem == null)
            {
                return NotFound();
            }

            await catalogRepository.RemoveItemAsync(id);

            await publishEndpoint.Publish(new CatalogItemsDeleted(existingItem.Id));

            return NoContent();
        }
    }
}
