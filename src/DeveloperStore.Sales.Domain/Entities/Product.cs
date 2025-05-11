using System;

namespace DeveloperStore.Sales.Domain.Entities
{
    /// <summary>
    /// Representa um produto disponível para venda.
    /// </summary>
    public class Product
    {
        /// <summary>ID do produto, gerado automaticamente.</summary>
        public Guid Id { get; private set; }

        /// <summary>Nome do produto (obrigatório).</summary>
        public string Name { get; private set; } = string.Empty;

        /// <summary>Preço unitário do produto (obrigatório e positivo).</summary>
        public decimal Price { get; private set; }

        /// <summary>Construtor privado para uso do EF Core.</summary>
        private Product() { }

        /// <summary>
        /// Cria um novo produto com nome e preço.
        /// </summary>
        public Product(string name, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome do produto é obrigatório.", nameof(name));

            if (price <= 0)
                throw new ArgumentException("O preço deve ser maior que zero.", nameof(price));

            Id = Guid.NewGuid();
            Name = name.Trim();
            Price = price;
        }

        /// <summary>Atualiza o nome do produto.</summary>
        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Nome inválido.", nameof(newName));

            Name = newName.Trim();
        }

        /// <summary>Atualiza o preço do produto.</summary>
        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice <= 0)
                throw new ArgumentException("O preço deve ser maior que zero.", nameof(newPrice));

            Price = newPrice;
        }
    }
}
