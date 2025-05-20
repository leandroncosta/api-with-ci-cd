using RelatoX.Application.DTOs;
using System.Text;

namespace RelatoX.Infra.Utils
{
    public static class CSVGenerator
    {
        public static byte[]? Generate<T>(IEnumerable<T> reports)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Month,TotalConsumption,EntryCount,Type");

            foreach (var report in reports)
            {
                dynamic r = report!;
                sb.AppendLine($"{r.Month},{r.TotalConsumption},{r.EntryCount},{r.Type}");
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        public static byte[]? GenerateByUser(MonthlyUserReportDto report)
        {
            var sb = new StringBuilder();
            sb.AppendLine("UserId,Year,Month,Water,Gas,Energy");

            foreach (var monthlyConsumptionSummary in report.Summary)
            {
                dynamic r = report!;
                sb.AppendLine($"{report.UserId},{report.Year},{monthlyConsumptionSummary.Month},{monthlyConsumptionSummary.Water},{monthlyConsumptionSummary.Gas},{monthlyConsumptionSummary.Energy}");
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }
    }
}