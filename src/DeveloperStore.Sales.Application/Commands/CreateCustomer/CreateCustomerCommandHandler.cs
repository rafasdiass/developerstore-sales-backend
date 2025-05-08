// src/DeveloperStore.Sales.Application/Commands/CreateCustomer/CreateCustomerCommandHandler.cs
using System;
using System.Threading;
using System.Threading.Tasks;
using DeveloperStore.Sales.Application.Repositories;
using DeveloperStore.Sales.Domain.Entities;

namespace DeveloperStore.Sales.Application.Commands.CreateCustomer
{
    /// <summary>
    /// Manipulador do comando de criação de cliente.
    /// </summary>
    public class CreateCustomerCommandHandler
    {
        private readonly ICustomerRepository _customerRepository;

        public CreateCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository
                ?? throw new ArgumentNullException(nameof(customerRepository));
        }

        public async Task<Guid> Handle(
            CreateCustomerCommand command,
            CancellationToken cancellationToken = default)
        {
            if (command is null)
                throw new ArgumentNullException(nameof(command));

            if (string.IsNullOrWhiteSpace(command.Name))
                throw new ArgumentException("O nome do cliente é obrigatório.", nameof(command.Name));

            var name = command.Name.Trim();
            var email = string.IsNullOrWhiteSpace(command.Email)
                ? null
                : command.Email.Trim();

            var customer = new Customer(name, email);

            await _customerRepository.AddAsync(customer);

            return customer.Id;
        }
    }
}
