// src/DeveloperStore.Sales.Application/Repositories/ICustomerRepository.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperStore.Sales.Domain.Entities;

namespace DeveloperStore.Sales.Application.Repositories
{
    /// <summary>
    /// Define as operações de persistência de Cliente.
    /// Camada de Application apenas expõe a interface, sem depender de EF ou Context.
    /// </summary>
    public interface ICustomerRepository
    {
        /// <summary>
        /// Retorna todos os clientes.
        /// </summary>
        Task<IReadOnlyList<Customer>> GetAllAsync();

        /// <summary>
        /// Retorna um cliente pelo ID, ou null se não existir.
        /// </summary>
        /// <param name="id">Identificador do cliente.</param>
        Task<Customer?> GetByIdAsync(Guid id);

        /// <summary>
        /// Adiciona um novo cliente.
        /// </summary>
        /// <param name="customer">Entidade cliente a ser adicionada.</param>
        Task AddAsync(Customer customer);
    }
}
