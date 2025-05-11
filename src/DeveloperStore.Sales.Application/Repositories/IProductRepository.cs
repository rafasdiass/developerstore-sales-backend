using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperStore.Sales.Domain.Entities;

namespace DeveloperStore.Sales.Application.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<Product>> GetAllAsync();
        Task AddAsync(Product product);
    }
}
