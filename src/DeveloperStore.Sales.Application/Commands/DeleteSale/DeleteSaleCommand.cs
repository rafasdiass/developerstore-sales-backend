// src/DeveloperStore.Sales.Application/Commands/DeleteSale/DeleteSaleCommand.cs
using System;

namespace DeveloperStore.Sales.Application.Commands.DeleteSale
{
    public class DeleteSaleCommand
    {
        public required Guid Id { get; init; }
    }
}
