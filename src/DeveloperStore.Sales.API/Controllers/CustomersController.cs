
using System;
using System.Threading;
using System.Threading.Tasks;
using DeveloperStore.Sales.API.Models;
using DeveloperStore.Sales.Application.Commands.CreateCustomer;
using DeveloperStore.Sales.Application.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperStore.Sales.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly CreateCustomerCommandHandler _createHandler;
        private readonly ICustomerRepository _customerRepository;

        public CustomersController(
            CreateCustomerCommandHandler createHandler,
            ICustomerRepository customerRepository)
        {
            _createHandler = createHandler ?? throw new ArgumentNullException(nameof(createHandler));
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        }

        /// <summary>
        /// Cria um novo cliente.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateCustomerCommand command,
            CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // executa validação e persistência
            var id = await _createHandler.Handle(command, ct);

            // retorna 201 Created com header Location e o corpo mínimo
            return CreatedAtRoute(
                routeName: "GetCustomerById",
                routeValues: new { id = id },
                value: new { id }
            );
        }

        /// <summary>
        /// Obtém um cliente pelo seu identificador.
        /// </summary>
        [HttpGet("{id:guid}", Name = "GetCustomerById")]
        public async Task<ActionResult<CustomerDto>> GetById(
            Guid id,
            CancellationToken ct = default)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer is null)
                return NotFound();

            // mapeia para DTO de saída
            var dto = new CustomerDto(
                Id: customer.Id,
                Name: customer.Name,
                Email: customer.Email
            );

            return Ok(dto);
        }
    }
}
