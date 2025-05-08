// src/DeveloperStore.Sales.Application/Commands/CreateBranch/CreateBranchCommand.cs
namespace DeveloperStore.Sales.Application.Commands.CreateBranch
{
    /// <summary>
    /// Dados necessários para criar uma filial.
    /// </summary>
    public record CreateBranchCommand(string Name);
}
