using System;
using System.Collections.Generic;

namespace DeveloperStore.Sales.Domain.Entities
{
    /// <summary>
    /// Representa um cliente no domínio de vendas.
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// ID do cliente, gerado automaticamente.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Nome completo do cliente (obrigatório).
        /// </summary>
        public string Name { get; private set; } = string.Empty;

        /// <summary>
        /// E-mail do cliente (opcional).
        /// </summary>
        public string? Email { get; private set; }

        // Navegação 1:N → Sales
        private readonly List<Sale> _sales = new();
        public IReadOnlyCollection<Sale> Sales => _sales.AsReadOnly();

        /// <summary>
        /// Construtor privado para o EF Core.
        /// </summary>
        private Customer()
        {
            // EF Core irá popular Id, Name e Email aqui
        }

        /// <summary>
        /// Cria um novo cliente.
        /// </summary>
        public Customer(string name, string? email = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome do cliente é obrigatório.", nameof(name));

            Id = Guid.NewGuid();
            Name = name.Trim();
            Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim();
        }

        /// <summary>
        /// Atualiza o nome do cliente.
        /// </summary>
        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Novo nome não pode ser vazio.", nameof(newName));

            Name = newName.Trim();
        }

        /// <summary>
        /// Atualiza o e-mail do cliente.
        /// </summary>
        public void UpdateEmail(string? newEmail)
        {
            Email = string.IsNullOrWhiteSpace(newEmail) ? null : newEmail.Trim();
        }
    }
}
