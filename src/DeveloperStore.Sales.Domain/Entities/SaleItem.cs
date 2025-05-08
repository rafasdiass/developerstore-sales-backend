using System;
using DeveloperStore.Sales.Domain.Services;

namespace DeveloperStore.Sales.Domain.Entities
{
    public class SaleItem
    {
        public Guid Id { get; private set; }
        public Guid SaleId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; } = null!;
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal Discount { get; private set; }
        public bool IsCancelled { get; private set; }

        public decimal TotalItemAmount
            => Math.Round((UnitPrice * Quantity) - Discount, 2);

        public Sale? Sale { get; private set; }

        private SaleItem() { } // EF Core

        public SaleItem(
            Guid productId,
            string productName,
            int quantity,
            decimal unitPrice,
            IDiscountCalculator discountCalculator)
        {
            if (string.IsNullOrWhiteSpace(productName))
                throw new ArgumentException("Nome do produto é obrigatório.", nameof(productName));
            if (quantity <= 0)
                throw new ArgumentException("Quantidade deve ser maior que zero.", nameof(quantity));
            if (unitPrice <= 0)
                throw new ArgumentException("Preço unitário deve ser maior que zero.", nameof(unitPrice));

            Id = Guid.NewGuid();
            ProductId = productId;
            ProductName = productName.Trim();
            Quantity = quantity;
            UnitPrice = unitPrice;
            Discount = discountCalculator.Calculate(quantity, unitPrice);
            IsCancelled = false;
        }

        public void Cancel()
        {
            if (!IsCancelled)
                IsCancelled = true;
        }
    }
}
