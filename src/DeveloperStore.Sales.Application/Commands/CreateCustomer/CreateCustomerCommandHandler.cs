
using System;
using System.Threading;
using System.Threading.Tasks;
using DeveloperStore.Sales.Application.Repositories;
using DeveloperStore.Sales.Domain.Entities;

namespace DeveloperStore.Sales.Application.Commands.CreateCustomer
{
    /// <summary>
    /// Manipulador do <see cref="CreateCustomerCommand"/>.
    /// Responsável apenas por:
    /// 1) Validar o comando
    /// 2) Criar a entidade Customer
    /// 3) Delegar a persistência ao repositório
    /// </summary>
    public class CreateCustomerCommandHandler
    {
        private readonly ICustomerRepository _customerRepository;

        /// <summary>
        /// Constrói o handler, injetando o repositório de clientes.
        /// </summary>
        /// <param name="customerRepository">Repositório de clientes.</param>
        public CreateCustomerCommandHandler(ICustomerRepository customerRepository)
            => _customerRepository = customerRepository
               ?? throw new ArgumentNullException(nameof(customerRepository));

        /// <summary>
        /// Executa a criação do cliente.
        /// </summary>
        /// <param name="command">Dados para criação.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>O <see cref="Guid"/> do cliente criado.</returns>
        /// <exception cref="ArgumentNullException">Se <paramref name="command"/> for nulo.</exception>
        /// <exception cref="ArgumentException">Se <paramref name="command.Name"/> for vazio ou whitespace.</exception>
        public async Task<Guid> Handle(
            CreateCustomerCommand command,
            CancellationToken cancellationToken = default)
        {
            if (command is null)
                throw new ArgumentNullException(nameof(command));

            if (string.IsNullOrWhiteSpace(command.Name))
                throw new ArgumentException(
                    "O nome do cliente é obrigatório.",
                    nameof(command.Name));

            // Trim para remover espaços acidentais
            var customerName = command.Name.Trim();

            // Cria a entidade de domínio
            var customer = new Customer(customerName);

            // Persiste via repositório
            await _customerRepository.AddAsync(customer);

            // Retorna o Id recém-gerado
            return customer.Id;
        }
    }
}
