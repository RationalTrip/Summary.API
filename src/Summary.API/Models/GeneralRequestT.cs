namespace Summary.API.Models;

public class GeneralRequestT<T>
    where T : class
{
    public T? Data { get; set; }
}
