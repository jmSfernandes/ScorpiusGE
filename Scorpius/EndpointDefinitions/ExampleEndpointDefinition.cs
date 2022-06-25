using CustomTemplate.Services;
using SleepDataExporter.Endpoints.Utils;

namespace CustomTemplate.EndpointDefinitions;

public class ExampleEndpointDefinition : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/exampleData", GetSleepData)
            .WithName("GetData").WithTags("Data");
    }

    public void DefineServices(IServiceCollection services)
    {
        //services.AddScoped<EstimationsService>();
        services.AddScoped<IExampleService, ExampleService>();
    }

    //use this if we want to use services instead of direct implementation
    private async Task<IResult> GetSleepData(IExampleService exampleService, string? id, string? dateFrom,
        string? dateTo)
    {
        return await exampleService.GetExampleData(id, dateFrom, dateTo);
    }
}