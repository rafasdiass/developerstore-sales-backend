using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeveloperStore.Sales.API.Models;
using DeveloperStore.Sales.Application.Repositories;
using DeveloperStore.Sales.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperStore.Sales.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        /// <summary>
        /// Cria um novo produto.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = new Product(request.Name, request.Price);
            await _productRepository.AddAsync(product);

            return Ok(new { id = product.Id });
        }

        /// <summary>
        /// Lista todos os produtos.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetAll()
        {
            var products = await _productRepository.GetAllAsync();

            var result = products
                .Select(p => new ProductDto(p.Id, p.Name, p.Price))
                .ToList();

            return Ok(result);
        }
    }
}
