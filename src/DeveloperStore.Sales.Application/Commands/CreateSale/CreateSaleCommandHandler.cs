using System;
using System.Threading.Tasks;
using DeveloperStore.Sales.Application.Repositories;
using DeveloperStore.Sales.Domain.Entities;
using DeveloperStore.Sales.Domain.Services;

namespace DeveloperStore.Sales.Application.Commands.CreateSale
{
    /// <summary>
    /// Handler responsável por processar o comando de criação de uma venda.
    /// </summary>
    public class CreateSaleCommandHandler
    {
        private readonly ISaleRepository _repository;
        private readonly IDiscountCalculator _discountCalculator;

        public CreateSaleCommandHandler(ISaleRepository repository, IDiscountCalculator discountCalculator)
        {
            _repository = repository;
            _discountCalculator = discountCalculator;
        }

        /// <summary>
        /// Executa o fluxo de criação da venda, instanciando Sale e SaleItem corretamente.
        /// </summary>
        public async Task<Guid> HandleAsync(CreateSaleCommand command)
        {
            // Cria o agregado raiz Sale com todos os parâmetros obrigatórios
            var sale = new Sale(
                command.SaleNumber,
                command.SaleDate,
                command.CustomerId,
                command.CustomerName,
                command.BranchId,
                command.BranchName
            );

            // Para cada item do comando, cria um SaleItem e adiciona na venda
            foreach (var itemCommand in command.Items)
            {
                var saleItem = new SaleItem(
                    itemCommand.ProductId,
                    itemCommand.ProductName,
                    itemCommand.Quantity,
                    itemCommand.UnitPrice,
                    _discountCalculator
                );
                sale.AddItem(saleItem);
            }

            // Persiste e retorna o Id gerado
            return await _repository.AddAsync(sale);
        }
    }
}
