using DeveloperStore.Sales.Application.Commands.CreateSale;
using DeveloperStore.Sales.Application.Repositories;
using DeveloperStore.Sales.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DeveloperStore.Sales.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly CreateSaleCommandHandler _createHandler;
        private readonly ISaleRepository _repository;

        public SalesController(
            CreateSaleCommandHandler createHandler,
            ISaleRepository repository)
        {
            _createHandler = createHandler;
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSale([FromBody] CreateSaleCommand command)
        {
            if (command == null)
                return BadRequest("Dados da venda são obrigatórios.");

            try
            {
                var id = await _createHandler.HandleAsync(command);
                return CreatedAtAction(nameof(GetSaleById), new { id }, new { id });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return UnprocessableEntity(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro inesperado: {ex.Message}");
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetSaleById(Guid id)
        {
            var sale = await _repository.GetByIdAsync(id);
            if (sale == null)
                return NotFound();

            var result = new
            {
                sale.Id,
                sale.SaleNumber,
                sale.SaleDate,
                sale.CustomerId,
                sale.CustomerName,
                sale.BranchId,
                sale.BranchName,
                sale.IsCancelled,
                sale.TotalAmount,
                Items = sale.Items.Select(i => new
                {
                    i.Id,
                    i.ProductId,
                    i.ProductName,
                    i.Quantity,
                    i.UnitPrice,
                    i.Discount,
                    i.TotalItemAmount,
                    i.IsCancelled
                })
            };

            return Ok(result);
        }
    }
}
