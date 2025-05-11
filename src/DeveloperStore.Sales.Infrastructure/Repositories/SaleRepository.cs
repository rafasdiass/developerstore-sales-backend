// src/DeveloperStore.Sales.Infrastructure/Repositories/SaleRepository.cs

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperStore.Sales.Application.Repositories;
using DeveloperStore.Sales.Domain.Entities;
using DeveloperStore.Sales.Domain.Services;
using DeveloperStore.Sales.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Sales.Infrastructure.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly SalesDbContext _context;

        public SaleRepository(SalesDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Guid> AddAsync(Sale sale)
        {
            if (sale is null)
                throw new ArgumentNullException(nameof(sale));

            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();
            return sale.Id;
        }

        public async Task<Sale?> GetByIdAsync(Guid id)
        {
            return await _context.Sales
                                 .Include(s => s.Items)
                                 .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Sale>> GetAllAsync()
        {
            return await _context.Sales
                                 .Include(s => s.Items)
                                 .ToListAsync();
        }

        public async Task UpdateAsync(Sale sale)
        {
            if (sale is null)
                throw new ArgumentNullException(nameof(sale));

          
            var existing = await _context.Sales
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == sale.Id);

            if (existing is null)
                throw new InvalidOperationException("Venda não encontrada para atualização.");

          
            _context.Entry(existing).CurrentValues.SetValues(sale);

           
            var itensAntigos = existing.Items.ToList(); 
            _context.SaleItems.RemoveRange(itensAntigos);

           
            foreach (var item in sale.Items)
            {
                _context.SaleItems.Add(item); 
            }

            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Guid id)
        {
            var sale = await _context.Sales.FindAsync(id);
            if (sale is null)
                throw new InvalidOperationException($"Venda com id '{id}' não encontrada.");

            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync();
        }
    }
}
