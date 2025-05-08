

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

        public CreateSaleCommandHandler(
            ISaleRepository repository,
            IDiscountCalculator discountCalculator)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _discountCalculator = discountCalculator ?? throw new ArgumentNullException(nameof(discountCalculator));
        }

        /// <summary>
        /// Executa o fluxo de criação da venda, instanciando Sale e SaleItem corretamente.
        /// </summary>
        /// <param name="command">Dados da nova venda (sem saleNumber, pois é gerado).</param>
        /// <returns>O Guid gerado para a nova venda.</returns>
        public async Task<Guid> HandleAsync(CreateSaleCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            // Cria o agregado raiz Sale;
            // o SaleNumber será gerado internamente
            var sale = new Sale(
                command.SaleDate,
                command.CustomerId,
                command.CustomerName,
                command.BranchId,
                command.BranchName
            );

            // Cria e adiciona cada item ao agregado
            foreach (var itemCmd in command.Items)
            {
                if (itemCmd == null)
                    throw new ArgumentException("Item de venda inválido.", nameof(command.Items));

                var saleItem = new SaleItem(
                    itemCmd.ProductId,
                    itemCmd.ProductName,
                    itemCmd.Quantity,
                    itemCmd.UnitPrice,
                    _discountCalculator
                );

                sale.AddItem(saleItem);
            }

            // Persiste a venda e retorna o Id
            return await _repository.AddAsync(sale);
        }
    }
}
