namespace Summary.API.Models;

public class GeneralErrorResponse
{
    public required string ErrorCode { get; set; }

    public required string Message { get; set; }

    public object? Details { get; set; }
}
