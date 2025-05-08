using System;
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
        Task<Customer?> GetByIdAsync(Guid id);
        Task AddAsync(Customer customer);
    }
}
