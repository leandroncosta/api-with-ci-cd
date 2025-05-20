namespace RelatoX.Application.DTOs

{
    public class MonthlyUserReportDto
    {
        public string UserId { get; set; } = default!;
        public int Year { get; set; }
        public List<MonthlyConsumptionSummaryDto> Summary { get; set; } = new();
    }

    public class MonthlyConsumptionSummaryDto
    {
        public string Month { get; set; } = default!;
        public decimal Water { get; set; }
        public decimal Gas { get; set; }
        public decimal Energy { get; set; }
    }
}