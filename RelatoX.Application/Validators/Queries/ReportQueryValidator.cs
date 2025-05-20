using FluentValidation;
using RelatoX.Application.DTOs.Queries;

namespace RelatoX.Application.Validators.Queries
{
    public class ReportQueryValidator : AbstractValidator<ReportQuery>
    {
        public ReportQueryValidator()
        {
            RuleFor(x => x.Month)
                .InclusiveBetween(1, 12).When(x => x.Month.HasValue)
                .WithMessage("Mês deve estar entre 1 e 12.");

            RuleFor(x => x.Type)
                    .IsInEnum().WithMessage("Tipo de consumo inválido. Deve ser water, energy ou gas");

            RuleFor(x => x.Year)
                .InclusiveBetween(2000, DateTime.Now.Year).When(x => x.Year.HasValue)
                .WithMessage($"Ano deve estar entre 2000 e {DateTime.Now.Year}.");

            RuleFor(x => x.Format)
                .Must(f => new[] { "json", "csv", "pdf" }.Contains(f.ToLower()))
                .WithMessage("Formato inválido. Use 'json', 'csv' ou 'pdf'.");
        }
    }
}