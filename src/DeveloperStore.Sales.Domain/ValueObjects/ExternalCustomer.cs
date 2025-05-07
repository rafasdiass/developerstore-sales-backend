using System;

namespace DeveloperStore.Sales.Domain.ValueObjects
{
    public readonly struct ExternalCustomer
    {
        public Guid Id { get; }
        public string Name { get; }

        public ExternalCustomer(Guid id, string name)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Customer ID cannot be empty.", nameof(id));

            Name = name ?? throw new ArgumentNullException(nameof(name));
            Id = id;
        }

        public override string ToString() => $"{Name} ({Id})";
    }
}
