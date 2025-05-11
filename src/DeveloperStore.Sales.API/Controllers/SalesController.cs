using System;
using System.Linq;
using System.Threading.Tasks;
using DeveloperStore.Sales.API.Models;
using DeveloperStore.Sales.Application.Commands.CreateSale;
using DeveloperStore.Sales.Application.Commands.UpdateSale;
using DeveloperStore.Sales.Application.Commands.DeleteSale;
using DeveloperStore.Sales.Application.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DeveloperStore.Sales.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly CreateSaleCommandHandler _createHandler;
        private readonly UpdateSaleCommandHandler _updateHandler;
        private readonly DeleteSaleCommandHandler _deleteHandler;
        private readonly ISaleRepository _repository;
        private readonly ILogger<SalesController> _logger;

        public SalesController(
            CreateSaleCommandHandler createHandler,
            UpdateSaleCommandHandler updateHandler,
            DeleteSaleCommandHandler deleteHandler,
            ISaleRepository repository,
            ILogger<SalesController> logger)
        {
            _createHandler = createHandler ?? throw new ArgumentNullException(nameof(createHandler));
            _updateHandler = updateHandler ?? throw new ArgumentNullException(nameof(updateHandler));
            _deleteHandler = deleteHandler ?? throw new ArgumentNullException(nameof(deleteHandler));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<IActionResult> CreateSale([FromBody] CreateSaleCommand command)
        {
            if (command is null)
            {
                _logger.LogWarning("Requisição de criação de venda com corpo nulo.");
                return BadRequest("Dados da venda são obrigatórios.");
            }

            try
            {
                _logger.LogInformation("Iniciando criação de nova venda.");
                var id = await _createHandler.HandleAsync(command);
                _logger.LogInformation("Venda criada com sucesso. ID: {SaleId}", id);

                return CreatedAtRoute(
                    routeName: nameof(GetSaleById),
                    routeValues: new { id },
                    value: new { id }
                );
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Erro de validação ao criar venda.");
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Operação inválida ao criar venda.");
                return UnprocessableEntity(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao criar venda.");
                return StatusCode(500, $"Erro inesperado: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSales()
        {
            _logger.LogInformation("Consultando todas as vendas.");

            var sales = await _repository.GetAllAsync();
            var result = sales
                .Select(sale => new SaleDto(
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
                ))
                .ToList();

            _logger.LogInformation("Total de vendas retornadas: {Count}", result.Count);
            return Ok(result);
        }

        [HttpGet("{id:guid}", Name = nameof(GetSaleById))]
        public async Task<IActionResult> GetSaleById(Guid id)
        {
            _logger.LogInformation("Consultando venda com ID: {SaleId}", id);

            var sale = await _repository.GetByIdAsync(id);
            if (sale is null)
            {
                _logger.LogWarning("Venda não encontrada: {SaleId}", id);
                return NotFound();
            }

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
            );

            _logger.LogInformation("Venda localizada: {SaleId}", id);
            return Ok(dto);
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> UpdateSale(Guid id, [FromBody] UpdateSaleCommand command)
        {
            if (command is null)
            {
                _logger.LogWarning("Requisição de atualização com corpo nulo. ID: {SaleId}", id);
                return BadRequest("Dados para atualização são obrigatórios.");
            }

            command.Id = id;
            _logger.LogInformation("Iniciando atualização da venda {SaleId}", id);

            try
            {
                if (command.SaleDate.HasValue)
                    _logger.LogInformation("Nova data da venda: {SaleDate}", command.SaleDate.Value);

                if (command.CustomerId.HasValue || !string.IsNullOrWhiteSpace(command.CustomerName))
                    _logger.LogInformation("Novo cliente: {CustomerId} - {CustomerName}", command.CustomerId, command.CustomerName);

                if (command.BranchId.HasValue || !string.IsNullOrWhiteSpace(command.BranchName))
                    _logger.LogInformation("Nova filial: {BranchId} - {BranchName}", command.BranchId, command.BranchName);

                if (command.Items is not null)
                    _logger.LogInformation("Itens enviados para atualização: {ItemCount}", command.Items.Count);

                await _updateHandler.HandleAsync(command);
                _logger.LogInformation("Venda {SaleId} atualizada com sucesso", id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Erro de argumento ao atualizar venda {SaleId}", id);
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Operação inválida ao atualizar venda {SaleId}", id);
                return UnprocessableEntity(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao atualizar venda {SaleId}", id);
                return StatusCode(500, $"Erro inesperado: {ex.Message}");
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteSale(Guid id)
        {
            _logger.LogInformation("Tentando excluir venda com ID: {SaleId}", id);

            try
            {
                await _deleteHandler.HandleAsync(new DeleteSaleCommand { Id = id });
                _logger.LogInformation("Venda {SaleId} excluída com sucesso", id);
                return NoContent();
            }
            catch (InvalidOperationException)
            {
                _logger.LogWarning("Tentativa de excluir venda inexistente: {SaleId}", id);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao excluir venda {SaleId}", id);
                return StatusCode(500, $"Erro inesperado: {ex.Message}");
            }
        }
    }
}
