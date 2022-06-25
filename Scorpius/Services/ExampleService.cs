using SleepDataExporter.Models;

namespace CustomTemplate.Services;

public class ExampleService : IExampleService
{
    private readonly IConfiguration _configuration;


    public ExampleService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<IResult> GetExampleData(string id, string dateFrom, string dateTo)
    {
        var lst = new List<SleepClassify>
        {
            new SleepClassify
            {
                Light = 6,

                Confidence = 80,
                Timestamp = "2022-05-01T14:00:00Z",
            }
        };

        if (lst.Count == 0)
            return Results.NotFound("the id is not valid or doesn't exist!");

        return Results.Json(lst);
    }
}