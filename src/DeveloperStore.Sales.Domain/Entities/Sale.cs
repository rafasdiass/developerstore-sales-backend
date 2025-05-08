

using System;
using System.Collections.Generic;
using System.Linq;

namespace DeveloperStore.Sales.Domain.Entities
{
    /// <summary>
    /// Representa uma venda, com referência a cliente, filial e seus itens.
    /// </summary>
    public class Sale
    {
        public Guid Id { get; private set; }

        /// <summary>
        /// Número da venda, gerado automaticamente.
        /// </summary>
        public string SaleNumber { get; private set; } = string.Empty;

        /// <summary>
        /// Data da venda.
        /// </summary>
        public DateTime SaleDate { get; private set; }

        /// <summary>
        /// FK para Cliente.
        /// </summary>
        public Guid CustomerId { get; private set; }

        /// <summary>
        /// Navegação para o cliente (nullable, preenchido pelo EF Core).
        /// </summary>
        public Customer? Customer { get; private set; }

        /// <summary>
        /// Nome do cliente.
        /// </summary>
        public string CustomerName { get; private set; } = string.Empty;

        /// <summary>
        /// FK para Filial.
        /// </summary>
        public Guid BranchId { get; private set; }

        /// <summary>
        /// Nome da filial.
        /// </summary>
        public string BranchName { get; private set; } = string.Empty;

        /// <summary>
        /// Indica se a venda foi cancelada.
        /// </summary>
        public bool IsCancelled { get; private set; }

        private readonly List<SaleItem> _items = new();
        public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

        /// <summary>
        /// Soma total da venda, considerando descontos de cada item.
        /// </summary>
        public decimal TotalAmount => _items.Sum(i => i.TotalItemAmount);

        /// <summary>
        /// Construtor privado para o EF Core.
        /// </summary>
        private Sale()
        {
            // EF Core irá popular todas as propriedades
        }

        /// <summary>
        /// Construtor principal: gera número e preenche todos os campos.
        /// </summary>
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

        /// <summary>
        /// Adiciona um item à venda.
        /// </summary>
        public void AddItem(SaleItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            _items.Add(item);
        }

        /// <summary>
        /// Cancela a venda e todos os seus itens.
        /// </summary>
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
