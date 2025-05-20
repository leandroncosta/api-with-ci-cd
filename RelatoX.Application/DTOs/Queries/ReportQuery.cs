using RelatoX.Domain.Enums;

namespace RelatoX.Application.DTOs.Queries
{
    public class ReportQuery
    {
        public int? Month { get; set; }
        public int? Year { get; set; }
        public ConsumptionType? Type { get; set; }
        public string Format { get; set; } = "json";
    }
}