using RelatoX.Domain.Enums;

namespace RelatoX.Domain.Entities
{
    public class ConsumptionEntry
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserId { get; set; }
        public ConsumptionType Type { get; set; }
        public double QuantityConsumed { get; set; }
        public string Unit { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}