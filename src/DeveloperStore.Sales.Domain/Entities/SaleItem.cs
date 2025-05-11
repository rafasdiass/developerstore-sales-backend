using System;
using DeveloperStore.Sales.Domain.Services;

namespace DeveloperStore.Sales.Domain.Entities
{
    public class SaleItem
    {
        public Guid Id { get; private set; }
        public Sale? Sale { get; private set; }
        public Guid SaleId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; } = string.Empty;
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal Discount { get; private set; }
        public bool IsCancelled { get; private set; }

        /// <summary>
        /// Timestamp da última modificação no item.
        /// </summary>
        public DateTime? UpdatedAt { get; private set; }

        /// <summary>
        /// Valor total do item com desconto.
        /// </summary>
        public decimal TotalItemAmount => Math.Round((UnitPrice * Quantity) - Discount, 2);

        private SaleItem() { } // Para uso do EF Core

        public SaleItem(
            Guid productId,
            string productName,
            int quantity,
            decimal unitPrice,
            IDiscountCalculator discountCalculator)
        {
            if (string.IsNullOrWhiteSpace(productName))
                throw new ArgumentException("Nome do produto é obrigatório.", nameof(productName));

            Id = Guid.NewGuid();
            ProductId = productId;
            ProductName = productName.Trim();
            Quantity = quantity;
            UnitPrice = unitPrice;
            Discount = discountCalculator.Calculate(quantity, unitPrice);
            IsCancelled = false;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Atualiza os dados do item de venda. Recalcula o desconto.
        /// </summary>
        public void Update(
            int quantity,
            decimal unitPrice,
            IDiscountCalculator discountCalculator)
        {
            Quantity = quantity;
            UnitPrice = unitPrice;
            Discount = discountCalculator.Calculate(quantity, unitPrice);
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Cancela o item de venda.
        /// </summary>
        public void Cancel()
        {
            IsCancelled = true;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
