using RelatoX.Domain.Enums;

namespace RelatoX.Application.DTOs
{
    public class ConsumptionPostDto
    {
        public string UserId { get; set; }
        public ConsumptionType Type { get; set; }
        public double QuantityConsumed { get; set; }

        /// <summary>
        /// // Data como string, no formato "yyyy/MM/dd" (ex: 2025/05/19)
        /// </summary>
        ///
        public string? Date { get; set; }
    }
}