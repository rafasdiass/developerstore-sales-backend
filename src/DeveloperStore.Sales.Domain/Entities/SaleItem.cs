using System;
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

        // Suprime aviso de não inicializado e será populado pelo EF Core
        public string ProductName { get; private set; } = null!;

        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal Discount { get; private set; }
        public bool IsCancelled { get; private set; }

        /// <summary>
        /// Valor total do item (quantidade * preço unitário - desconto).
        /// </summary>
        public decimal TotalItemAmount => Math.Round((UnitPrice * Quantity) - Discount, 2);

        // Construtor para EF Core materializar a entidade
        private SaleItem() { }

        /// <summary>
        /// Cria um novo item de venda, aplicando as regras de desconto.
        /// </summary>
        /// <param name="productId">ID do produto</param>
        /// <param name="productName">Nome do produto</param>
        /// <param name="quantity">Quantidade vendida</param>
        /// <param name="unitPrice">Preço unitário</param>
        /// <param name="discountCalculator">Serviço de cálculo de desconto</param>
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

            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            UnitPrice = unitPrice;
            Discount = discountCalculator.Calculate(quantity, unitPrice);
            IsCancelled = false;
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Cancela o item da venda, se ainda não estiver cancelado.
        /// </summary>
        public void Cancel()
        {
            if (!IsCancelled)
                IsCancelled = true;
        }
    }
}
