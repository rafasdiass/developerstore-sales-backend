using System;
using System.Linq;
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _customerRepository.GetAllAsync();
            var dtos = customers
                .Select(c => new CustomerDto(c.Id, c.Name, c.Email))
                .ToList();

            return Ok(dtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateCustomerCommand command, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var id = await _createHandler.Handle(command, ct);

                return CreatedAtRoute("GetCustomerById", new { id }, new { id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpGet("{id:guid}", Name = "GetCustomerById")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct = default)
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer is null)
                return NotFound();

            return Ok(new CustomerDto(customer.Id, customer.Name, customer.Email));
        }
    }
}
