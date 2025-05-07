using System;
using System.Collections.Generic;
using System.Linq;

namespace DeveloperStore.Sales.Domain.Entities
{
    /// <summary>
    /// Representa uma venda com seus itens e dados de cliente e filial.
    /// </summary>
    public class Sale
    {
        public Guid Id { get; private set; }
        public string SaleNumber { get; private set; }
        public DateTime SaleDate { get; private set; }

        public Guid CustomerId { get; private set; }
        public string CustomerName { get; private set; }

        public Guid BranchId { get; private set; }
        public string BranchName { get; private set; }

        public bool IsCancelled { get; private set; }

        private readonly List<SaleItem> _items = new List<SaleItem>();
        public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

        /// <summary>
        /// Soma total da venda considerando os descontos de cada item.
        /// </summary>
        public decimal TotalAmount => _items.Sum(i => i.TotalItemAmount);

        /// <summary>
        /// Construtor principal da venda.
        /// </summary>
        /// <param name="saleNumber">Número da venda</param>
        /// <param name="saleDate">Data da venda</param>
        /// <param name="customerId">ID do cliente</param>
        /// <param name="customerName">Nome do cliente</param>
        /// <param name="branchId">ID da filial</param>
        /// <param name="branchName">Nome da filial</param>
        public Sale(
            string saleNumber,
            DateTime saleDate,
            Guid customerId,
            string customerName,
            Guid branchId,
            string branchName)
        {
            if (string.IsNullOrWhiteSpace(saleNumber))
                throw new ArgumentException("O número da venda é obrigatório.", nameof(saleNumber));

            if (string.IsNullOrWhiteSpace(customerName))
                throw new ArgumentException("O nome do cliente é obrigatório.", nameof(customerName));

            if (string.IsNullOrWhiteSpace(branchName))
                throw new ArgumentException("O nome da filial é obrigatório.", nameof(branchName));

            Id = Guid.NewGuid();
            SaleNumber = saleNumber;
            SaleDate = saleDate;
            CustomerId = customerId;
            CustomerName = customerName;
            BranchId = branchId;
            BranchName = branchName;
            IsCancelled = false;
        }

        /// <summary>
        /// Adiciona um item à venda.
        /// </summary>
        /// <param name="item">Item de venda</param>
        public void AddItem(SaleItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

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
            {
                item.Cancel();
            }
        }
    }
}
