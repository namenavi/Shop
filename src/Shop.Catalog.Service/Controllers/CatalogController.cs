using Microsoft.AspNetCore.Mvc;
using Shop.Catalog.Service.Entities;
using Shop.Catalog.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Shop.Catalog.Service
{
    [Route("items")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private static readonly List<CatalogItem> items = new()
        {
            new CatalogItem() {
                Id = Guid.NewGuid(),
                Name="Велосипед Стелс",
                Description="Очень быстрый",
                Price= 22_000,
                CatalogType = new CatalogType() {
                    Id = Guid.NewGuid(),
                    Type="Велосипеды"
                }
            },
            new CatalogItem() {
                Id = Guid.NewGuid(),
                Name="Юбка крокс",
                Description="Стильная",
                Price= 2_500,
                CatalogType = new CatalogType() {
                    Id = Guid.NewGuid(),
                    Type="Одежда"
                }
            },
            new CatalogItem() {
                Id = Guid.NewGuid(),
                Name="Телефон Саммунг",
                Description="Много памяти",
                Price= 13_000,
                CatalogType = new CatalogType() {
                    Id = Guid.NewGuid(),
                    Type="Телефоны"
                }
            }
        };

        // GET: /items/
        [HttpGet]
        public IEnumerable<CatalogItem> Get()
        {
            return items;
        }

        // GET /items/5546
        [HttpGet("{id}")]
        public ActionResult<CatalogItem> GetById(Guid id)
        {
            var item = items.Where(item => item.Id == id).SingleOrDefault();
            if(item == null)
            {
                return NotFound();
            }
            return item!;
        }

        // POST /items/
        [HttpPost]
        public ActionResult<CatalogItem> Post(CreateCatalogItemDTO createItemDTO)
        {
            var item = new CatalogItem()
            {
                Id = Guid.NewGuid(),
                Name = createItemDTO.Name,
                Description = createItemDTO.Description,
                Price = createItemDTO.Price,
                CatalogTypeId = createItemDTO.CatalogTypeId
            };

            items.Add(item);

            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        // PUT /items/{id}
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, UpdateCatalogItemDTO updateItemDTO)
        {
            var existingItem = items.Where(item => item.Id == id).SingleOrDefault();
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
                items.Add(item);
                return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
            }

            existingItem.Price = updateItemDTO.Price;
            existingItem.Name = updateItemDTO.Name;
            existingItem.Description = updateItemDTO.Description;
            existingItem.CatalogTypeId = updateItemDTO.CatalogTypeId;

            return NoContent();
        }

        // DELETE /items/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var index = items.FindIndex(item => item.Id == id);
            if(index == -1)
            {
                return NotFound();
            }

            items.RemoveAt(index);

            return NoContent();
        }
    }
}
