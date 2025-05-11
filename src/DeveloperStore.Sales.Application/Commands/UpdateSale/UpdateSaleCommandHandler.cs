using System;
using System.Linq;
using System.Threading.Tasks;
using DeveloperStore.Sales.Application.Commands.UpdateSale;
using DeveloperStore.Sales.Application.Repositories;
using DeveloperStore.Sales.Domain.Entities;
using DeveloperStore.Sales.Domain.Services;

namespace DeveloperStore.Sales.Application.Commands.UpdateSale
{
    public class UpdateSaleCommandHandler
    {
        private readonly ISaleRepository _repository;
        private readonly IDiscountCalculator _discountCalculator;

        public UpdateSaleCommandHandler(
            ISaleRepository repository,
            IDiscountCalculator discountCalculator)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _discountCalculator = discountCalculator ?? throw new ArgumentNullException(nameof(discountCalculator));
        }

        public async Task HandleAsync(UpdateSaleCommand command)
        {
            if (command is null)
                throw new ArgumentNullException(nameof(command));

            var sale = await _repository.GetByIdAsync(command.Id)
                ?? throw new InvalidOperationException("Venda não encontrada.");

            // Atualiza apenas campos presentes (parcial)
            if (command.SaleDate.HasValue)
                sale.UpdateSaleDate(command.SaleDate.Value);

            if (command.CustomerId.HasValue && !string.IsNullOrWhiteSpace(command.CustomerName))
                sale.UpdateCustomer(command.CustomerId.Value, command.CustomerName!);

            if (command.BranchId.HasValue && !string.IsNullOrWhiteSpace(command.BranchName))
                sale.UpdateBranch(command.BranchId.Value, command.BranchName!);

            if (command.Items is not null && command.Items.Any())
            {
                sale.ClearItems();
                foreach (var itemCmd in command.Items)
                {
                    var item = new SaleItem(
                        itemCmd.ProductId,
                        itemCmd.ProductName,
                        itemCmd.Quantity,
                        itemCmd.UnitPrice,
                        _discountCalculator
                    );
                    sale.AddItem(item);
                }
            }

            await _repository.UpdateAsync(sale);
        }
    }
}
