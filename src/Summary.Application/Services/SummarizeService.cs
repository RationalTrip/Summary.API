using FluentValidation;
using Summary.Core.Exceptions;
using Summary.Core.Interfaces;
using Summary.Core.Models;

namespace Summary.Application.Services;

public class SummarizeService : ISummarizeService
{
    private readonly IValidator<SummarizeRequest> _validator;
    private readonly ILanguageServiceCaller _languageServiceCaller;

    public SummarizeService(
        IValidator<SummarizeRequest> validator,
        ILanguageServiceCaller languageServiceCaller)
    {
        _validator = validator;
        _languageServiceCaller = languageServiceCaller;
    }

    public async Task<SummarizeResponse> SummarizeAsync(SummarizeRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(e => new { e.PropertyName, e.ErrorMessage })
                .ToList();

            throw new InvalidModelSummaryException("Validation failed.", errors);
        }

        var summary = await _languageServiceCaller.AbstractiveSummarizeAsync(request.InputText, cancellationToken);

        return new SummarizeResponse { Summaries = summary };
    }
}
