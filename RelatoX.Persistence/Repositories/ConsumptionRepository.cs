using Microsoft.EntityFrameworkCore;
using RelatoX.Application.Interfaces;
using RelatoX.Domain.Entities;
using RelatoX.Domain.Enums;

namespace RelatoX.Persistence.Repositories
{
    public class ConsuptionRepository : IConsumptionRepository
    {
        private readonly ApplicationDbContext _context;

        public ConsuptionRepository(ApplicationDbContext context)
        {
            _context = context;
            SeedData(1000);
        }

        public async Task<ConsumptionEntry> AddAsync(ConsumptionEntry entry)
        {
            await _context.AddAsync(entry);
            await _context.SaveChangesAsync();
            return entry;
        }

        public async Task<List<ConsumptionEntry>> GetAllAsync(int finalPage, int finalPageSize, string? userId, int? month, int? year)

        {
            var query = _context.Consumptions.AsQueryable();

            if (userId != null) query = query.Where(x => x.UserId == userId);
            if (month != null) query = query.Where(x => x.Date.Month == month);
            if (year != null) query = query.Where(x => x.Date.Year == year);

            var result = await query.Skip((finalPage - 1) * finalPageSize).Take(finalPageSize)
                 .OrderBy(o => o.Date.Month).ToListAsync();

            return result;
        }

        public Task<List<ConsumptionEntry>> GetByTypeAndYearAsync(int? year, ConsumptionType type)
        {
            var query = _context.Consumptions.AsQueryable();

            if (year != null) query = query.Where(x => x.Date.Year.Equals(year));
            if (type != null) query = query.Where(x => x.Type.Equals(type));

            var result = query.OrderBy(o => o.Date.Month);
            return result.ToListAsync();
        }

        public Task<List<ConsumptionEntry>> GetByUser(string userId)
        {
            var query = _context.Consumptions.AsQueryable();
            var result = query.Where(x => x.UserId == userId).ToListAsync();
            return result;
        }

        public void SeedData(int quant)
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string numbers = "0123456789";

            int totalUsuarios = (int)Math.Ceiling(quant / 5.0);
            var userIds = Enumerable.Range(0, totalUsuarios)
                                    .Select(_ => GenerateId())
                                    .ToList();

            for (int i = 0; i < quant; i++)
            {
                _context.Consumptions.Add(new ConsumptionEntry
                {
                    UserId = userIds[Random.Shared.Next(userIds.Count)],
                    QuantityConsumed = Random.Shared.Next(0, 350),
                    Date = GenerateDate(2025),
                    Type = GenerateConsumptionType()
                });
            }
        }

        #region Metodos Privados

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

        private DateTime GenerateDate(int ano)
        {
            int diasNoAno = DateTime.IsLeapYear(ano) ? 366 : 365;
            int diasAleatorios = Random.Shared.Next(0, diasNoAno);

            return new DateTime(ano, 1, 1).AddDays(diasAleatorios);
        }

        public Task<List<ConsumptionEntry>> GetAllAsync(int finalPage, int finalPageSize, int? month, int? year, ConsumptionType? type)
        {
            throw new NotImplementedException();
        }

        public Task<List<ConsumptionEntry>> GetAllByUserAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ConsumptionEntry>> GetConsumptionReportAsync(int? year, int? month, ConsumptionType? type)
        {
            throw new NotImplementedException();
        }

        #endregion Metodos Privados
    }
}