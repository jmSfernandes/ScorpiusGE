namespace CustomTemplate.Services;

public interface IExampleService
{
    Task<IResult> GetExampleData(string id, string dateFrom, string dateTo);
}