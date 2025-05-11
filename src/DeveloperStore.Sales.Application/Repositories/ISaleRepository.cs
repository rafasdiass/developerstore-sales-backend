using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperStore.Sales.Domain.Entities;

namespace DeveloperStore.Sales.Application.Repositories
{
    public interface ISaleRepository
    {
        Task<Guid> AddAsync(Sale sale);
        Task<Sale?> GetByIdAsync(Guid id);
        Task<IEnumerable<Sale>> GetAllAsync();
        Task UpdateAsync(Sale sale);
        Task DeleteAsync(Guid id);
    }
}