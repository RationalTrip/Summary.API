using FluentValidation;
using Microsoft.Extensions.Options;
using Summary.Application.Configurations;
using Summary.Core.Models;

namespace Summary.Application.Validators;

public class SummarizeRequestValidator : AbstractValidator<SummarizeRequest>
{
    public SummarizeRequestValidator(IOptions<SummarizeConfigurations> options)
    {
        var config = options.Value;

        RuleFor(x => x.InputText)
            .NotEmpty()
            .WithMessage("Input text is required.");

        RuleFor(x => x.InputText)
            .MaximumLength(config.MaxInputLength)
            .WithMessage($"Input text must not exceed {config.MaxInputLength} characters.");
    }
}
