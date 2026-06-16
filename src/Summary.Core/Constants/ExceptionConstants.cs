namespace Summary.Core.Constants;

public class ExceptionConstants
{

    public const int BadRequestStatusCode = 400;

    public const int UnauthorizedStatusCode = 401;

    /// <summary>
    /// Error code for code execution errors, like Null Reference Exception, Index Out of Range Exception, etc.
    /// </summary>
    public const string UnexpectedErrorCode = "001";

    public const string GeneralErrorCode = "002";

    public const string InvalidModelErrorCode = "003";

    public const string UnauthorizedErrorCode = "004";
}
