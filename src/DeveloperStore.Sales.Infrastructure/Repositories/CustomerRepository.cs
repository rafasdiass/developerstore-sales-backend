using System;
using System.Threading.Tasks;
using DeveloperStore.Sales.Application.Repositories;
using DeveloperStore.Sales.Domain.Entities;
using DeveloperStore.Sales.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Sales.Infrastructure.Repositories
{
    /// <summary>
    /// Implementação EF Core de ICustomerRepository.
    /// Segue SRP: só lida com Customer no DbContext.
    /// </summary>
    public class CustomerRepository : ICustomerRepository
    {
        private readonly SalesDbContext _ctx;

        public CustomerRepository(SalesDbContext ctx)
        {
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        }

        public async Task AddAsync(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            await _ctx.Customers.AddAsync(customer);
            await _ctx.SaveChangesAsync();
        }

        public Task<Customer?> GetByIdAsync(Guid id)
        {
            // AsNoTracking porque só vamos ler sem alterar
            return _ctx.Customers
                       .AsNoTracking()
                       .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
