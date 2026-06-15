using Summary.API.Models;

namespace Summary.API.Extensions;

public static class GeneralRequesExtensions
{
    public static T GetNotNullBody<T>(this GeneralRequestT<T> request) where T : class
    {
        // ArgumentException is thrown because it not a user input error, but a bad user input wrapper
        if (request.Data is null)
            throw new ArgumentException("Request model validation Error. 'Data' section is not passed.");

        return request.Data;
    }
}
