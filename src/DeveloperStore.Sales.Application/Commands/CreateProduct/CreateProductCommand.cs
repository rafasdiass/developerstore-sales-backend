// src/DeveloperStore.Sales.Application/Commands/CreateProduct/CreateProductCommand.cs

namespace DeveloperStore.Sales.Application.Commands.CreateProduct
{
    /// <summary>
    /// Dados necessários para criar um produto.
    /// </summary>
    public record CreateProductCommand(string Name, decimal Price);
}
