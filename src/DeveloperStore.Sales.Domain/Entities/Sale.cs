using System;
using System.Collections.Generic;
using System.Linq;
using DeveloperStore.Sales.Domain.Services;

namespace DeveloperStore.Sales.Domain.Entities
{
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

        private readonly List<SaleItem> _items = new();
        public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

        public decimal TotalAmount => _items.Sum(i => i.TotalItemAmount);

        private Sale() { } // EF Core

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
            SaleDate = saleDate;
            CustomerId = customerId;
            CustomerName = customerName.Trim();
            BranchId = branchId;
            BranchName = branchName.Trim();
            IsCancelled = false;
            SaleNumber = GenerateSaleNumber();
        }

        public void AddItem(SaleItem item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            _items.Add(item);
        }

        public void Cancel()
        {
            if (IsCancelled) return;
            IsCancelled = true;
            foreach (var item in _items)
                item.Cancel();
        }

        private static string GenerateSaleNumber()
        {
            var ts = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var guidPart = Guid.NewGuid().ToString("N")[..5].ToUpperInvariant();
            return $"S{ts}-{guidPart}";
        }
    }
}
