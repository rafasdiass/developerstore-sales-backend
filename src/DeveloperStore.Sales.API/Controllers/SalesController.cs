using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeveloperStore.Sales.API.Models;
using DeveloperStore.Sales.Application.Commands.CreateSale;
using DeveloperStore.Sales.Application.Repositories;
using Microsoft.AspNetCore.Mvc;

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
            _createHandler = createHandler ?? throw new ArgumentNullException(nameof(createHandler));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>POST /api/sales</summary>
        [HttpPost]
        public async Task<IActionResult> CreateSale([FromBody] CreateSaleCommand command)
        {
            if (command is null)
                return BadRequest("Dados da venda são obrigatórios.");

            try
            {
                var id = await _createHandler.HandleAsync(command);
                return CreatedAtRoute(
                    routeName: nameof(GetSaleById),
                    routeValues: new { id },
                    value: new { id }
                );
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

        /// <summary>GET /api/sales</summary>
        [HttpGet]
        public async Task<IActionResult> GetAllSales()
        {
            var sales = await _repository.GetAllAsync();

            var result = sales.Select(sale => new SaleDto(
                sale.Id,
                sale.SaleNumber,
                sale.SaleDate,
                sale.CustomerId,
                sale.CustomerName,
                sale.BranchId,
                sale.BranchName,
                sale.IsCancelled,
                sale.TotalAmount,
                sale.Items.Select(i => new SaleItemDto(
                    i.Id,
                    i.ProductId,
                    i.ProductName,
                    i.Quantity,
                    i.UnitPrice,
                    i.Discount,
                    i.TotalItemAmount,
                    i.IsCancelled
                )).ToList().AsReadOnly()
            )).ToList();

            return Ok(result);
        }

        /// <summary>GET /api/sales/{id}</summary>
        [HttpGet("{id:guid}", Name = nameof(GetSaleById))]
        public async Task<IActionResult> GetSaleById(Guid id)
        {
            var sale = await _repository.GetByIdAsync(id);
            if (sale is null)
                return NotFound();

            var itemsDto = sale.Items
                .Select(i => new SaleItemDto(
                    i.Id,
                    i.ProductId,
                    i.ProductName,
                    i.Quantity,
                    i.UnitPrice,
                    i.Discount,
                    i.TotalItemAmount,
                    i.IsCancelled
                ))
                .ToList()
                .AsReadOnly();

            var dto = new SaleDto(
                sale.Id,
                sale.SaleNumber,
                sale.SaleDate,
                sale.CustomerId,
                sale.CustomerName,
                sale.BranchId,
                sale.BranchName,
                sale.IsCancelled,
                sale.TotalAmount,
                itemsDto
            );

            return Ok(dto);
        }
    }
}
