using RelatoX.Application.Interfaces;
using RelatoX.Domain.Entities;
using RelatoX.Domain.Enums;
using System.Net.Quic;

namespace RelatoX.Infra.Data.InMemory
{
    public class InMemoryConsumptionRepository : IConsumptionRepository
    {
        private readonly List<ConsumptionEntry> _data = new();

        public InMemoryConsumptionRepository()

        {
            SeedData(1000);
        }

        public Task<ConsumptionEntry> AddAsync(ConsumptionEntry entry)
        {
            _data.Add(entry);
            return Task.FromResult(entry);
        }

        public Task<List<ConsumptionEntry>> GetAllAsync(int finalPage, int finalPageSize, int? month, int? year, ConsumptionType? type)

        {
            var query = _data.AsQueryable();

            if (month != null) query = query.Where(x => x.Date.Month == month);
            if (year != null) query = query.Where(x => x.Date.Year == year);
            if (type != null) query = query.Where(x => x.Type == type);

            query = query.Skip((finalPage - 1) * finalPageSize).Take(finalPageSize);

            return Task.FromResult(query.ToList());
        }

        public Task<List<ConsumptionEntry>> GetConsumptionReportAsync(int? year, int? month, ConsumptionType? type)
        {
            var query = _data.AsQueryable();

            if (year != null) query = query.Where(x => x.Date.Year.Equals(year));
            if (month != null) query = query.Where(x => x.Date.Month.Equals(month));
            if (type != null) query = query.Where(x => x.Type.Equals(type));

            query = query.OrderBy(o => o.Date.Month);
            return Task.FromResult(query.ToList());
        }

        public Task<List<ConsumptionEntry>> GetAllByUserAsync(string userId)
        {
            var query = _data.AsQueryable();
            query = query.Where(x => x.UserId == userId);
            return Task.FromResult(query.ToList());
        }

        public void SeedData(int quant)
        {
            int totalUsuarios = (int)Math.Ceiling(quant / 5.0);
            var userIds = Enumerable.Range(0, totalUsuarios)
                                    .Select(_ => GenerateId())
                                    .ToList();

            for (int i = 0; i < quant; i++)
            {
                var type = GenerateConsumptionType();
                _data.Add(new ConsumptionEntry
                {
                    UserId = userIds[Random.Shared.Next(userIds.Count)],
                    QuantityConsumed = Random.Shared.Next(0, 350),
                    Date = GenerateDate(),
                    Type = type,
                    Unit = GetUnit(type)
                });
            }
        }

        #region Metodos Privados

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

        private string GenerateId(int length = 6)
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string numbers = "0123456789";
            var random = new Random();
            return new string(
                Enumerable.Repeat(letters + numbers, length)
                .Select(s => s[random.Next(s.Length)]).ToArray()
                );
        }

        private ConsumptionType GenerateConsumptionType()
        {
            var types = Enum.GetValues(typeof(ConsumptionType));
            return (ConsumptionType)types.GetValue(Random.Shared.Next(types.Length));
        }

        private DateTime GenerateDate()
        {
            int anoAtual = DateTime.Now.Year;
            int anoAleatorio = Random.Shared.Next(2020, anoAtual + 1);
            int diasNoAno = DateTime.IsLeapYear(anoAleatorio) ? 366 : 365;
            int diasAleatorios = Random.Shared.Next(0, diasNoAno);

            return new DateTime(anoAleatorio, 1, 1).AddDays(diasAleatorios);
        }

        #endregion Metodos Privados
    }
}