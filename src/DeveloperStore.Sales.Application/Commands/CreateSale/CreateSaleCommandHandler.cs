using System;
using System.Threading.Tasks;
using DeveloperStore.Sales.Application.Repositories;
using DeveloperStore.Sales.Domain.Entities;
using DeveloperStore.Sales.Domain.Services;

namespace DeveloperStore.Sales.Application.Commands.CreateSale
{
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

        public async Task<Guid> HandleAsync(CreateSaleCommand command)
        {
            if (command is null)
                throw new ArgumentNullException(nameof(command));

            if (command.Items is null || command.Items.Count == 0)
                throw new ArgumentException("A venda deve ter pelo menos um item.", nameof(command.Items));

            var sale = new Sale(
                command.SaleDate,
                command.CustomerId,
                command.CustomerName,
                command.BranchId,
                command.BranchName
            );

            foreach (var itemCmd in command.Items)
            {
                if (itemCmd is null)
                    throw new ArgumentException("Item de venda não pode ser nulo.", nameof(command.Items));

                var saleItem = new SaleItem(
                    itemCmd.ProductId,
                    itemCmd.ProductName,
                    itemCmd.Quantity,
                    itemCmd.UnitPrice,
                    _discountCalculator
                );

                sale.AddItem(saleItem);
            }

            return await _repository.AddAsync(sale);
        }
    }
}
