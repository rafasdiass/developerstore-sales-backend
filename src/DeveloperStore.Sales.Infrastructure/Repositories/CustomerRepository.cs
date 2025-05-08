// src/DeveloperStore.Sales.Infrastructure/Repositories/CustomerRepository.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperStore.Sales.Application.Repositories;
using DeveloperStore.Sales.Domain.Entities;
using DeveloperStore.Sales.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Sales.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly SalesDbContext _context;

        public CustomerRepository(SalesDbContext context)
            => _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<IReadOnlyList<Customer>> GetAllAsync()
        {
            return await _context.Customers
                                 .AsNoTracking()
                                 .ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            return await _context.Customers
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
        }
    }
}
