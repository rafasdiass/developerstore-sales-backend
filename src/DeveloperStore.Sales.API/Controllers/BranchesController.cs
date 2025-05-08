using Microsoft.AspNetCore.Mvc;
using DeveloperStore.Sales.Application.Commands.CreateBranch;
using DeveloperStore.Sales.Application.Repositories;

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
            _repository = repository;
            _createHandler = createHandler;
        }

        /// <summary>GET /api/branches</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var branches = await _repository.GetAllAsync();
            var result = branches
                .Select(b => new { b.Id, b.Name })
                .ToList();

            return Ok(result);
        }

        /// <summary>POST /api/branches</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBranchCommand command)
        {
            if (string.IsNullOrWhiteSpace(command?.Name))
                return BadRequest("Nome da filial é obrigatório.");

            var id = await _createHandler.HandleAsync(command);
            return CreatedAtAction(
                nameof(GetAll),
                new { id },
                new { id }
            );
        }
    }
}
