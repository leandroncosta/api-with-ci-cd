using RelatoX.Domain.Enums;

namespace RelatoX.Application.DTOs.Queries
{
    public class ConsumptionQuery
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public ConsumptionType? Type { get; set; }
    }
}