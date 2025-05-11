// src/DeveloperStore.Sales.Application/Commands/DeleteSale/DeleteSaleCommandHandler.cs
using System;
using System.Threading.Tasks;
using DeveloperStore.Sales.Application.Commands.DeleteSale;
using DeveloperStore.Sales.Application.Repositories;

namespace DeveloperStore.Sales.Application.Commands.DeleteSale
{
    public class DeleteSaleCommandHandler
    {
        private readonly ISaleRepository _repository;

        public DeleteSaleCommandHandler(ISaleRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task HandleAsync(DeleteSaleCommand command)
        {
            if (command is null)
                throw new ArgumentNullException(nameof(command));

            var existing = await _repository.GetByIdAsync(command.Id);
            if (existing is null)
                throw new InvalidOperationException("Venda não encontrada.");

            await _repository.DeleteAsync(command.Id);
        }
    }
}
