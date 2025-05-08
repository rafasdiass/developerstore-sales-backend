
using System;

namespace DeveloperStore.Sales.Application.Commands.CreateCustomer
{
    /// <summary>
    /// Comando para criação de um novo cliente.
    /// Contém apenas os dados necessários para a operação.
    /// </summary>
    public sealed record CreateCustomerCommand(string Name);
}
