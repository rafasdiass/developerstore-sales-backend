// src/DeveloperStore.Sales.API/Controllers/ProductsController.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeveloperStore.Sales.API.Models;
using DeveloperStore.Sales.Application.Commands.CreateProduct;
using DeveloperStore.Sales.Application.Repositories;
using Microsoft.AspNetCore.Mvc;


namespace DeveloperStore.Sales.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly CreateProductCommandHandler _createHandler;

        public ProductsController(
            IProductRepository repository,
            CreateProductCommandHandler createHandler)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _createHandler = createHandler ?? throw new ArgumentNullException(nameof(createHandler));
        }

        /// <summary>GET /api/products</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _repository.GetAllAsync();

            var result = products
                .Select(p => new ProductDto(p.Id, p.Name, p.Price))
                .ToList()
                .AsReadOnly();

            return Ok(result);
        }

        /// <summary>POST /api/products</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
        {
            if (command == null || string.IsNullOrWhiteSpace(command.Name))
                return BadRequest("Nome do produto é obrigatório.");

            var id = await _createHandler.HandleAsync(command);

            return CreatedAtAction(
                actionName: nameof(GetAll),
                routeValues: new { id },
                value: new { id }
            );
        }
    }
}
