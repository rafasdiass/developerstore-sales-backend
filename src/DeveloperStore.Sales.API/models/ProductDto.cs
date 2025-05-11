// src/DeveloperStore.Sales.API/Models/ProductDto.cs
using System;

namespace DeveloperStore.Sales.API.Models
{
    public record ProductDto(Guid Id, string Name, decimal Price);
}
