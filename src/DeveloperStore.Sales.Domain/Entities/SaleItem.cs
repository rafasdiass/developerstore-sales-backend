

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DeveloperStore.Sales.Domain.Services;

namespace DeveloperStore.Sales.Domain.Entities
{
    /// <summary>
    /// Representa um item dentro de uma venda.
    /// </summary>
    public class SaleItem
    {
        public Guid Id { get; private set; }
        public Guid ProductId { get; private set; }
        public required string ProductName { get; init; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal Discount { get; private set; }
        public bool IsCancelled { get; private set; }

        /// <summary>
        /// Valor total do item (quantidade * preço unitário - desconto).
        /// </summary>
        public decimal TotalItemAmount
            => Math.Round((UnitPrice * Quantity) - Discount, 2);

        /// <summary>
        /// Construtor privado usado pelo EF Core
        /// </summary>
        private SaleItem() { }

        /// <summary>
        /// Construtor principal que aplica as regras de negócio e preenche todos os membros.
        /// </summary>
        [SetsRequiredMembers]
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

        /// <summary>
        /// Cancela o item de venda.
        /// </summary>
        public void Cancel()
        {
            if (!IsCancelled) IsCancelled = true;
        }
    }
}
