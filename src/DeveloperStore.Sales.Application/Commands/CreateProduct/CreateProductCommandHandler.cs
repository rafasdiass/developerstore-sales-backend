using System;
using System.Threading.Tasks;
using DeveloperStore.Sales.Application.Repositories;
using DeveloperStore.Sales.Domain.Entities;

namespace DeveloperStore.Sales.Application.Commands.CreateProduct
{
    /// <summary>
    /// Handler para o comando de criação de produto.
    /// </summary>
    public class CreateProductCommandHandler
    {
        private readonly IProductRepository _repository;

        public CreateProductCommandHandler(IProductRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Guid> HandleAsync(CreateProductCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Name))
                throw new ArgumentException("Nome do produto é obrigatório.", nameof(command.Name));

            if (command.Price <= 0)
                throw new ArgumentException("Preço do produto deve ser maior que zero.", nameof(command.Price));

            var product = new Product(command.Name, command.Price);
            await _repository.AddAsync(product);
            return product.Id;
        }
    }
}
