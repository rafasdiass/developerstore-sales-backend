
using System;

namespace DeveloperStore.Sales.API.Models
{
    /// <summary>
    /// DTO de leitura para uma filial.
    /// </summary>
    public sealed record BranchDto(
        Guid Id,
        string Name
    );
}
