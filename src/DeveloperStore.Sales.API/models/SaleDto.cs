
using System;
using System.Collections.Generic;

namespace DeveloperStore.Sales.API.Models
{
    /// <summary>
    /// DTO de leitura para uma venda.
    /// </summary>
    public sealed record SaleDto(
        Guid Id,
        string SaleNumber,
        DateTime SaleDate,
        Guid CustomerId,
        string CustomerName,
        Guid BranchId,
        string BranchName,
        bool IsCancelled,
        decimal TotalAmount,
        IReadOnlyCollection<SaleItemDto> Items
    );
}
