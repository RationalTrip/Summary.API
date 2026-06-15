using Summary.Core.Constants;

namespace Summary.Core.Exceptions;

public class SummaryGeneralException : SummaryBaseException
{
    public SummaryGeneralException(string message, object? details = null, object? debugDetails = null)
        : base(message, ExceptionConstants.BadRequestStatusCode, ExceptionConstants.GeneralErrorCode, details, debugDetails)
    {

    }
}
