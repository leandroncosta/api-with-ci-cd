using RelatoX.Application.DTOs;
using RelatoX.Domain.Entities;
using RelatoX.Domain.Enums;

namespace RelatoX.Application.Interfaces
{
    public interface IConsumptionService
    {
        Task<ConsumptionEntry> AddConsumptionAsync(ConsumptionPostDto dto);

        Task<List<ConsumptionEntry>> GetAllConsumptionAsync(int? finalPage, int? finalPageSize, int? month, int? year, ConsumptionType? type);

        Task<List<ReportDto>> GetConsumptionReportAsync(int? year, int? month, ConsumptionType? type);

        Task<MonthlyUserReportDto> GetConsumptionReportByUserAsync(string userId);
    }
}