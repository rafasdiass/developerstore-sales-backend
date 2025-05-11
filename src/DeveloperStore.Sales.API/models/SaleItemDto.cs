
namespace DeveloperStore.Sales.API.Models
{
    /// <summary>
    /// DTO de leitura para um item de venda.
    /// </summary>
    public sealed record SaleItemDto(
        Guid Id,
        Guid ProductId,
        string ProductName,
        int Quantity,
        decimal UnitPrice,
        decimal Discount,
        decimal TotalItemAmount,
        bool IsCancelled
    );
}
