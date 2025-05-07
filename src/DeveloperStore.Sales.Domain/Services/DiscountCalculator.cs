using System;

namespace DeveloperStore.Sales.Domain.Services
{
    /// <summary>
    /// Implementação da lógica de negócio de desconto com base na quantidade de itens vendidos.
    /// Aplica as seguintes regras:
    /// - < 4 unidades: sem desconto
    /// - 4 a 9 unidades: 10% de desconto
    /// - 10 a 20 unidades: 20% de desconto
    /// - > 20 unidades: lançamento de exceção
    /// </summary>
    public class DiscountCalculator : IDiscountCalculator
    {
        public decimal Calculate(int quantity, decimal unitPrice)
        {
            ValidateInputs(quantity, unitPrice);

            decimal total = unitPrice * quantity;

            if (quantity >= 10 && quantity <= 20)
                return Math.Round(total * 0.20m, 2);

            if (quantity >= 4)
                return Math.Round(total * 0.10m, 2);

            return 0m;
        }

        private void ValidateInputs(int quantity, decimal unitPrice)
        {
            if (quantity <= 0)
                throw new ArgumentException("A quantidade deve ser maior que zero.", nameof(quantity));

            if (unitPrice <= 0)
                throw new ArgumentException("O preço unitário deve ser maior que zero.", nameof(unitPrice));

            if (quantity > 20)
                throw new InvalidOperationException("A venda não pode conter mais de 20 unidades do mesmo produto.");
        }
    }
}
