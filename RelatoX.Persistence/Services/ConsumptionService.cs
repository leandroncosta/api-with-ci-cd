using RelatoX.Application.DTOs;
using RelatoX.Application.Interfaces;
using RelatoX.Domain.Entities;
using RelatoX.Domain.Enums;
using System.Globalization;

namespace RelatoX.Persistence.Services
{
    public class ConsumptionService : IConsumptionService
    {
        private readonly IConsumptionRepository _consumptionRepository;

        public ConsumptionService(IConsumptionRepository consumptionRepository)
        {
            _consumptionRepository = consumptionRepository;
        }

        public async Task<ConsumptionEntry> AddConsumptionAsync(ConsumptionPostDto dto)
        {
            var date = DateTime.ParseExact(dto.Date ?? DateTime.UtcNow.ToString("yyyy/MM/dd"), "yyyy/MM/dd", CultureInfo.InvariantCulture);
            var dateOffset = new DateTimeOffset(date, TimeZoneInfo.Local.GetUtcOffset(date));

            ConsumptionEntry entry = new ConsumptionEntry()
            {
                UserId = dto.UserId,
                Type = dto.Type,
                QuantityConsumed = dto.QuantityConsumed,
                Unit = GetUnit(dto.Type),
                Date = dateOffset
            };

            await _consumptionRepository.AddAsync(entry);

            return entry;
        }

        public async Task<List<ConsumptionEntry>> GetAllConsumptionAsync(int? page, int? pageSize, int? month, int? year, ConsumptionType? type)
        {
            var finalPageSize = Math.Clamp(pageSize ?? 100, 1, 100); // mínimo 1, máximo 100

            var consumptions = await _consumptionRepository.GetAllAsync(page ?? 1, finalPageSize, month, year, type);
            return consumptions;
        }

        public async Task<List<ReportDto>> GetConsumptionReportAsync(int? year, int? month, ConsumptionType? type)
        {
            var data = await _consumptionRepository.GetConsumptionReportAsync(year ?? DateTime.Now.Year, month, type);

            var newReports = data.GroupBy(x => new { x.Date.Month, x.Type })
            .Select((x, index) => new ReportDto()
            {
                Year = year ?? DateTime.Now.Year,
                Month = x.Key.Month,
                TotalConsumption = (decimal)x.Sum(i => i.QuantityConsumed),
                EntryCount = x.Count(),
                Type = x.Key.Type,
                Unit = GetUnit(x.First().Type),
                TotalCost = (decimal)x.Sum(e => e.QuantityConsumed * GetTariffPerUnit(x.First().Type))
            }).ToList();

            return newReports;
        }

        public async Task<MonthlyUserReportDto> GetConsumptionReportByUserAsync(string userId)
        {
            var consumptionsByUser = await _consumptionRepository.GetAllByUserAsync(userId);

            var summaries = consumptionsByUser.GroupBy(x => x.Date.Month)
                .Select(x => new MonthlyConsumptionSummaryDto()
                {
                    //Month = x.Key.ToString(),
                    //Water = x.Sum(i => i.Type == ConsumptionType.Water ? i.QuantityConsumed : 0),
                    //Energy = x.Sum(i => i.Type == ConsumptionType.Energy ? i.QuantityConsumed : 0),
                    //Gas = x.Sum(i => i.Type == ConsumptionType.Gas ? i.QuantityConsumed : 0)
                }).ToList();

            var result = new MonthlyUserReportDto()
            {
                UserId = userId,
                Year = DateTime.Now.Year,
                Summary = summaries
            };

            return result;
        }

        private static string GetUnit(ConsumptionType type)
        {
            return type switch
            {
                ConsumptionType.Water => "liters",
                ConsumptionType.Gas => "m³",
                ConsumptionType.Energy => "kWh",
                _ => "unknown"
            };
        }

        public static double GetTariffPerUnit(ConsumptionType type) => type switch
        {
            ConsumptionType.Water => 0.005,   // R$0.005 por litro
            ConsumptionType.Gas => 2.75,      // R$2.75 por m³
            ConsumptionType.Energy => 0.80,   // R$0.80 por kWh
            _ => 0
        };
    }
}