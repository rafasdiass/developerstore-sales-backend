
using System;

namespace DeveloperStore.Sales.Domain.Entities
{
    /// <summary>
    /// Entidade que representa uma Filial.
    /// </summary>
    public class Branch
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }

        public Branch(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome da filial é obrigatório.", nameof(name));

            Id = Guid.NewGuid();
            Name = name;
        }
    }
}
