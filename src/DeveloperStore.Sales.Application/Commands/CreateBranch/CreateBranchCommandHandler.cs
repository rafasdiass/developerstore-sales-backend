// src/DeveloperStore.Sales.Application/Commands/CreateBranch/CreateBranchCommandHandler.cs
using System;
using System.Threading.Tasks;
using DeveloperStore.Sales.Application.Repositories;
using DeveloperStore.Sales.Domain.Entities;

namespace DeveloperStore.Sales.Application.Commands.CreateBranch
{
    /// <summary>
    /// Handler para o comando de criação de filial.
    /// </summary>
    public class CreateBranchCommandHandler
    {
        private readonly IBranchRepository _repository;

        public CreateBranchCommandHandler(IBranchRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> HandleAsync(CreateBranchCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Name))
                throw new ArgumentException("Nome da filial é obrigatório.", nameof(command.Name));

            var branch = new Branch(command.Name);
            await _repository.AddAsync(branch);
            return branch.Id;
        }
    }
}
