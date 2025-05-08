// src/DeveloperStore.Sales.Application/Commands/CreateCustomer/CreateCustomerCommand.cs
using System;

namespace DeveloperStore.Sales.Application.Commands.CreateCustomer
{
    /// <summary>
    /// Comando para criação de um novo cliente.
    /// </summary>
    public sealed record CreateCustomerCommand(string Name, string? Email);
}
