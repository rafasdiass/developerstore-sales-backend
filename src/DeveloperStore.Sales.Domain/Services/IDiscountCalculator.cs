namespace DeveloperStore.Sales.Domain.Services
{
    /// <summary>
    /// Contrato para serviços que realizam o cálculo de desconto com base na quantidade e preço unitário.
    /// </summary>
    public interface IDiscountCalculator
    {
        /// <summary>
        /// Calcula o valor de desconto aplicado com base nas regras de quantidade.
        /// </summary>
        /// <param name="quantity">Quantidade de itens</param>
        /// <param name="unitPrice">Preço unitário</param>
        /// <returns>Valor do desconto a ser subtraído</returns>
        decimal Calculate(int quantity, decimal unitPrice);
    }
}
