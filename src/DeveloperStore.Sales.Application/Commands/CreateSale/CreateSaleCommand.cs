// src/DeveloperStore.Sales.Application/Commands/CreateSale/CreateSaleCommand.cs
using System;
using System.Collections.Generic;

namespace DeveloperStore.Sales.Application.Commands.CreateSale
{
    /// <summary>
    /// Representa os dados necessários para criar uma venda.
    /// </summary>
    public class CreateSaleCommand
    {
        public required DateTime SaleDate { get; set; }

        public required Guid CustomerId { get; set; }
        public required string CustomerName { get; set; }

        public required Guid BranchId { get; set; }
        public required string BranchName { get; set; }

        public List<CreateSaleItemCommand> Items { get; set; } = new();
    }

    /// <summary>
    /// Representa os dados necessários para adicionar um item à venda.
    /// </summary>
    public class CreateSaleItemCommand
    {
        public required Guid ProductId { get; set; }
        public required string ProductName { get; set; }
        public required int Quantity { get; set; }
        public required decimal UnitPrice { get; set; }
        public decimal Discount { get; set; } = 0; // Garantir que esteja compatível com o domínio
    }
}
