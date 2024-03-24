using Microsoft.AspNetCore.Mvc;
using Shop.Catalog.Service.Model;
using System;
using System.Collections.Generic;


namespace Shop.Catalog.Service
{
    [Route("items")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private static readonly List<CatalogItem> items = new()
        {
            new CatalogItem() { Id = Guid.NewGuid(), Name=string.Empty, Description=string.Empty, Price= 1_000},
            new CatalogItem() {},
            new CatalogItem() {},
        };
        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
