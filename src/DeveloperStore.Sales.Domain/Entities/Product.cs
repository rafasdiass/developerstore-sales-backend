using System;

namespace DeveloperStore.Sales.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public decimal Price { get; private set; }

        private Product() { }

        public Product(string name, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome do produto é obrigatório.", nameof(name));
            if (price <= 0)
                throw new ArgumentException("Preço deve ser maior que zero.", nameof(price));

            Id = Guid.NewGuid();
            Name = name.Trim();
            Price = price;
        }
    }
}
