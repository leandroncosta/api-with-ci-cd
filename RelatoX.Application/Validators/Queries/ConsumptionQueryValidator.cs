using FluentValidation;
using RelatoX.Application.DTOs.Queries;

namespace RelatoX.Application.Validators.Queries
{
    public class ConsumptionQueryValidator : AbstractValidator<ConsumptionQuery>
    {
        public ConsumptionQueryValidator()
        {
            //RuleFor(x => x.Page)
            //    .GreaterThan(0).When(x => x.Page.HasValue)
            //    .WithMessage("Page deve ser maior que 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).When(x => x.PageSize.HasValue)
                .WithMessage("PageSize deve estar entre 1 e 100.");

            RuleFor(x => x.Month)
                .InclusiveBetween(1, 12).When(x => x.Month.HasValue)
                .WithMessage("Mês deve estar entre 1 e 12.");

            RuleFor(x => x.Type)
                     .IsInEnum().WithMessage("Tipo de consumo inválido. Deve ser water, energy ou gas");

            RuleFor(x => x.Year)
                .InclusiveBetween(2000, DateTime.Now.Year).When(x => x.Year.HasValue)
                .WithMessage($"Ano deve estar entre 2000 e {DateTime.Now.Year}.");
        }
    }
}