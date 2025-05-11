using System;
using System.Collections.Generic;
using DeveloperStore.Sales.Application.Commands.CreateSale;

namespace DeveloperStore.Sales.Application.Commands.UpdateSale
{
    public class UpdateSaleCommand
    {
        /// <summary>
        /// Identificador da venda a ser atualizada. Agora setável pelo Controller.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nova data da venda (opcional).
        /// </summary>
        public DateTime? SaleDate { get; set; }

        /// <summary>
        /// Novo ID do cliente (opcional).
        /// </summary>
        public Guid? CustomerId { get; set; }

        /// <summary>
        /// Novo nome do cliente (opcional).
        /// </summary>
        public string? CustomerName { get; set; }

        /// <summary>
        /// Novo ID da filial (opcional).
        /// </summary>
        public Guid? BranchId { get; set; }

        /// <summary>
        /// Novo nome da filial (opcional).
        /// </summary>
        public string? BranchName { get; set; }

        /// <summary>
        /// Itens atualizados da venda (opcional). Se nulo, mantém os itens existentes.
        /// </summary>
        public List<CreateSaleItemCommand>? Items { get; set; }
    }
}
