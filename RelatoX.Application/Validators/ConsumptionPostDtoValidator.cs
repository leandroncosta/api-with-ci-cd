using FluentValidation;
using RelatoX.Application.DTOs;
using System.Globalization;

namespace RelatoX.Application.Validators
{
    public class ConsumptionPostDtoValidator : AbstractValidator<ConsumptionPostDto>
    {
        public ConsumptionPostDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("O campo 'UserId' é obrigatório.");

            RuleFor(x => x.Type)
                  .NotEmpty().WithMessage("O campo 'Type' é obrigatório.")
                       .IsInEnum().WithMessage("Tipo de consumo inválido.");

            RuleFor(x => x.QuantityConsumed)
                 .NotEmpty().WithMessage("O campo 'QuantityConsumed' é obrigatório.")
          .GreaterThan(0).WithMessage("Quantidade consumida deve ser maior que zero.");

            RuleFor(x => x.Date)
             .Cascade(CascadeMode.Stop)
             .NotEmpty().When(x => !string.IsNullOrEmpty(x.Date)) 
             .Matches(@"^\d{4}/\d{2}/\d{2}$").WithMessage("A data deve estar no formato yyyy/MM/dd.")
             .Must(dateStr => BeAValidDate(dateStr)).WithMessage("Data inválida ou maior que hoje.")
             .When(x => !string.IsNullOrEmpty(x.Date)); 

        }

        private bool BeAValidDate(string dateStr)
        {
            if (string.IsNullOrEmpty(dateStr)) return false;

            bool parsed = DateTime.TryParseExact(dateStr,
                                                 "yyyy/MM/dd",
                                                 CultureInfo.InvariantCulture,
                                                 DateTimeStyles.None,
                                                 out DateTime date);

            if (!parsed) return false;
            return date.Date <= DateTime.Today;
        }
    }
}