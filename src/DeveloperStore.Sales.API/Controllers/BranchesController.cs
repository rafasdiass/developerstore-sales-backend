
using System;
using System.Linq;
using System.Threading.Tasks;
using DeveloperStore.Sales.API.Models;
using DeveloperStore.Sales.Application.Commands.CreateBranch;
using DeveloperStore.Sales.Application.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperStore.Sales.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BranchesController : ControllerBase
    {
        private readonly IBranchRepository _repository;
        private readonly CreateBranchCommandHandler _createHandler;

        public BranchesController(
            IBranchRepository repository,
            CreateBranchCommandHandler createHandler)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _createHandler = createHandler ?? throw new ArgumentNullException(nameof(createHandler));
        }

        /// <summary>GET /api/branches</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var branches = await _repository.GetAllAsync();

            // Mapeia entidade -> DTO
            var result = branches
                .Select(b => new BranchDto(b.Id, b.Name))
                .ToList()
                .AsReadOnly();

            return Ok(result);
        }

        /// <summary>POST /api/branches</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBranchCommand command)
        {
            if (command == null || string.IsNullOrWhiteSpace(command.Name))
                return BadRequest("Nome da filial é obrigatório.");

            var id = await _createHandler.HandleAsync(command);

            return CreatedAtAction(
                actionName: nameof(GetAll),
                routeValues: new { id },
                value: new { id }
            );
        }
    }
}
