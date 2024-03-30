using Microsoft.AspNetCore.Mvc;
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
        private readonly IRepository<CatalogItem> _catalogRepository;
        public CatalogController(IRepository<CatalogItem> catalogRepository)
        {
            this._catalogRepository = catalogRepository;
        }

        // GET: /items/
        [HttpGet]
        public async Task<IEnumerable<CatalogItem>> GetAsync()
        {
            var items = await _catalogRepository.GetItemsAsync();
            return items;
        }

        // GET /items/5546
        [HttpGet("{id}")]
        public async Task<ActionResult<CatalogItem>> GetByIdAsync(Guid id)
        {
            var item = await _catalogRepository.GetItemAsync(id);
            if(item == null)
            {
                return NotFound();
            }
            return item!;
        }

        // POST /items/
        [HttpPost]
        public async Task<ActionResult<CatalogItem>> PostAsync(CreateCatalogItemDTO createItemDTO)
        {
            var item = new CatalogItem()
            {
                Id = Guid.NewGuid(),
                Name = createItemDTO.Name,
                Description = createItemDTO.Description,
                Price = createItemDTO.Price,
                CatalogTypeId = createItemDTO.CatalogTypeId
            };

            await _catalogRepository.CreateItemsAsync(item);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
        }

        // PUT /items/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdateCatalogItemDTO updateItemDTO)
        {
            var existingItem = await _catalogRepository.GetItemAsync(id);

            if(existingItem == null)
            {
                var item = new CatalogItem
                {
                    Id= id,
                    Name = updateItemDTO.Name,
                    Description = updateItemDTO.Description,
                    Price = updateItemDTO.Price,
                    CatalogTypeId = updateItemDTO.CatalogTypeId
                };

                await _catalogRepository.CreateItemsAsync(item);
                return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
            }

            existingItem.Price = updateItemDTO.Price;
            existingItem.Name = updateItemDTO.Name;
            existingItem.Description = updateItemDTO.Description;
            existingItem.CatalogTypeId = updateItemDTO.CatalogTypeId;
            await _catalogRepository.UpdateItemAsync(existingItem);

            return NoContent();
        }

        // DELETE /items/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var existingItem = await _catalogRepository.GetItemAsync(id);

            if(existingItem == null)
            {
                return NotFound();
            }

            await _catalogRepository.RemoveItemAsync(id);

            return NoContent();
        }
    }
}
