using RelatoX.Domain.Entities;
using RelatoX.Domain.Enums;

namespace RelatoX.Application.Interfaces

{
    public interface IConsumptionRepository
    {
        Task<ConsumptionEntry> AddAsync(ConsumptionEntry entry);

        Task<List<ConsumptionEntry>> GetAllAsync(int finalPage, int finalPageSize, int? month, int? year, ConsumptionType? type);

        Task<List<ConsumptionEntry>> GetAllByUserAsync(string userId);

        Task<List<ConsumptionEntry>> GetConsumptionReportAsync(int? year, int? month, ConsumptionType? type);
    }
}