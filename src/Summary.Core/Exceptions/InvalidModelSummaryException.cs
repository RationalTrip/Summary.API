using Summary.Core.Constants;

namespace Summary.Core.Exceptions;

public class InvalidModelSummaryException : SummaryBaseException
{
    public InvalidModelSummaryException(string message, object? details)
        : base(message, ExceptionConstants.BadRequestStatusCode, ExceptionConstants.InvalidModelErrorCode, details)
    {

    }
}
