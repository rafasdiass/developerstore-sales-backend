
namespace DeveloperStore.Sales.API.Models
{
    /// <summary>
    /// DTO de leitura para Cliente.
    /// </summary>
    public sealed record CustomerDto(Guid Id, string Name, string? Email);
}
