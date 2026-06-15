namespace Summary.Core.Exceptions;

public class SummaryBaseException : Exception
{
    public SummaryBaseException(string message, int statusCode, string errorCode, object? details, object? debugDetails)
        : base(message)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
        Details = details;
        DebugDetails = debugDetails;
    }

    public int StatusCode { get; set; }

    public string ErrorCode { get; set; }

    public object? Details { get; set; }

    public object? DebugDetails { get; set; }
}
