// src/DeveloperStore.Sales.API/Controllers/DiscountsController.cs
using DeveloperStore.Sales.Application.Queries.CalculateDiscount;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DeveloperStore.Sales.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscountsController : ControllerBase
    {
        private readonly CalculateDiscountQueryHandler _handler;

        public DiscountsController(CalculateDiscountQueryHandler handler)
            => _handler = handler;

        /// <summary>
        /// Ex.: GET /api/discounts?quantity=5&unitPrice=12.34
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int quantity, [FromQuery] decimal unitPrice)
        {
            try
            {
                var result = await _handler.HandleAsync(quantity, unitPrice);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return UnprocessableEntity(ex.Message);
            }
        }
    }
}
