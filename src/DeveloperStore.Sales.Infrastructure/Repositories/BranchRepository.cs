// src/DeveloperStore.Sales.Infrastructure/Repositories/BranchRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperStore.Sales.Application.Repositories;
using DeveloperStore.Sales.Domain.Entities;
using DeveloperStore.Sales.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Sales.Infrastructure.Repositories
{
    /// <summary>
    /// Implementação EF de IBranchRepository.
    /// </summary>
    public class BranchRepository : IBranchRepository
    {
        private readonly SalesDbContext _db;

        public BranchRepository(SalesDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Branch branch)
        {
            _db.Branches.Add(branch);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Branch>> GetAllAsync()
        {
            return await _db.Branches.AsNoTracking().ToListAsync();
        }
    }
}
