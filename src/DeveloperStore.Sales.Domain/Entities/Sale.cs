using System;
using System.Collections.Generic;
using System.Linq;
using DeveloperStore.Sales.Domain.Services;

namespace DeveloperStore.Sales.Domain.Entities
{
    /// <summary>
    /// Representa uma venda no domínio.
    /// </summary>
    public class Sale
    {
        public Guid Id { get; private set; }
        public string SaleNumber { get; private set; } = string.Empty;
        public DateTime SaleDate { get; private set; }
        public Guid CustomerId { get; private set; }
        public Customer? Customer { get; private set; }
        public string CustomerName { get; private set; } = string.Empty;
        public Guid BranchId { get; private set; }
        public string BranchName { get; private set; } = string.Empty;
        public bool IsCancelled { get; private set; }

        /// <summary>
        /// Timestamp de última modificação.
        /// </summary>
        public DateTime? UpdatedAt { get; private set; }

        private readonly List<SaleItem> _items = new();
        public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

        public decimal TotalAmount => _items.Sum(i => i.TotalItemAmount);

        private Sale() { } // para EF Core

        public Sale(
            DateTime saleDate,
            Guid customerId,
            string customerName,
            Guid branchId,
            string branchName)
        {
            if (string.IsNullOrWhiteSpace(customerName))
                throw new ArgumentException("Nome do cliente é obrigatório.", nameof(customerName));
            if (string.IsNullOrWhiteSpace(branchName))
                throw new ArgumentException("Nome da filial é obrigatório.", nameof(branchName));

            Id = Guid.NewGuid();
            SaleNumber = GenerateSaleNumber();
            SaleDate = saleDate;
            CustomerId = customerId;
            CustomerName = customerName.Trim();
            BranchId = branchId;
            BranchName = branchName.Trim();
            IsCancelled = false;
            UpdatedAt = null;
        }

        public void AddItem(SaleItem item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            _items.Add(item);
            UpdatedAt = DateTime.UtcNow;
        }

        public void ClearItems()
        {
            _items.Clear();
            UpdatedAt = DateTime.UtcNow;
        }

        public void Cancel()
        {
            if (IsCancelled) return;

            IsCancelled = true;
            foreach (var item in _items)
                item.Cancel();

            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateSaleDate(DateTime newDate)
        {
            if (SaleDate != newDate)
            {
                SaleDate = newDate;
                UpdatedAt = DateTime.UtcNow;
            }
        }

        public void UpdateCustomer(Guid customerId, string customerName)
        {
            if (CustomerId != customerId || CustomerName != customerName)
            {
                CustomerId = customerId;
                CustomerName = customerName.Trim();
                UpdatedAt = DateTime.UtcNow;
            }
        }

        public void UpdateBranch(Guid branchId, string branchName)
        {
            if (BranchId != branchId || BranchName != branchName)
            {
                BranchId = branchId;
                BranchName = branchName.Trim();
                UpdatedAt = DateTime.UtcNow;
            }
        }

        private static string GenerateSaleNumber()
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var suffix = Guid.NewGuid().ToString("N")[..5].ToUpperInvariant();
            return $"S{timestamp}-{suffix}";
        }
    }
}
