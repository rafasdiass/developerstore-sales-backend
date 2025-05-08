using System;
using System.Collections.Generic;

namespace DeveloperStore.Sales.Domain.Entities
{
    /// <summary>
    /// Representa um cliente no domínio de vendas.
    /// </summary>
    public class Customer
    {
        /// <summary>ID do cliente, gerado automaticamente.</summary>
        public Guid Id { get; private set; }

        /// <summary>Nome completo do cliente (obrigatório).</summary>
        public string Name { get; private set; } = string.Empty;

        /// <summary>E-mail do cliente (opcional).</summary>
        public string? Email { get; private set; }

        private readonly List<Sale> _sales = new();
        public IReadOnlyCollection<Sale> Sales => _sales.AsReadOnly();

        private Customer() { }

        public Customer(string name, string? email = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome do cliente é obrigatório.", nameof(name));

            Id = Guid.NewGuid();
            Name = name.Trim();
            Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim();
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Novo nome não pode ser vazio.", nameof(newName));

            Name = newName.Trim();
        }

        public void UpdateEmail(string? newEmail)
        {
            Email = string.IsNullOrWhiteSpace(newEmail) ? null : newEmail.Trim();
        }
    }
}
