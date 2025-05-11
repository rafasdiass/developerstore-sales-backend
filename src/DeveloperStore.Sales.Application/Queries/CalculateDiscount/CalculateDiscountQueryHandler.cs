// src/DeveloperStore.Sales.Application/Queries/CalculateDiscount/CalculateDiscountQueryHandler.cs
using DeveloperStore.Sales.Domain.Services;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperStore.Sales.Application.Queries.CalculateDiscount
{
    public class CalculateDiscountQueryHandler
    {
        private readonly IDiscountCalculator _calculator;

        public CalculateDiscountQueryHandler(IDiscountCalculator calculator)
            => _calculator = calculator;

        public Task<DiscountResult> HandleAsync(int quantity, decimal unitPrice, CancellationToken ct = default)
        {
            var discount = _calculator.Calculate(quantity, unitPrice);
            return Task.FromResult(new DiscountResult(discount));
        }
    }
}
