
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperStore.Sales.Domain.Entities;

namespace DeveloperStore.Sales.Application.Repositories
{
    /// <summary>
    /// Contrato para persistência de filiais.
    /// </summary>
    public interface IBranchRepository
    {
        Task AddAsync(Branch branch);
        Task<IEnumerable<Branch>> GetAllAsync();
        // Se desejar: Task<Branch?> GetByIdAsync(Guid id);
    }
}
