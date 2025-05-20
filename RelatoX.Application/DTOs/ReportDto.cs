using RelatoX.Domain.Enums;

namespace RelatoX.Application.DTOs
{
    public class ReportDto
    {
        /// <summary>
        /// // ano do relatorio
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// // Mes do relatorio
        /// </summary>
        ///
        public int Month { get; set; }

        /// <summary>
        /// // Mes do relatorio nome
        /// </summary>
        ///
        public string MonthName => new DateTime(Year, Month, 1).ToString("MMMM");

        /// <summary>
        /// Tipo de consumo (Water, Gas, Energy)
        /// </summary>
        public ConsumptionType Type { get; set; }

        /// <summary>
        /// unidade de media do (Water, Gas, Energy)
        /// </summary>
        public string Unit { get; set; } = null!;

        /// <summary>
        /// // Soma dos valores de consumo no mês
        /// </summary>
        public decimal TotalConsumption { get; set; }

        /// <summary>
        /// Quantidade de registros no mês
        /// </summary>
        public int EntryCount { get; set; }

        /// <summary>
        /// Qquntidade em R$
        /// </summary>
        public decimal TotalCost { get; set; }
    }
}