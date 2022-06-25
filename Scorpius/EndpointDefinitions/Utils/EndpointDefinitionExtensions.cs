namespace SleepDataExporter.Endpoints.Utils;

public static class EndpointDefinitionExtensions
{
    public static void AddAllEndpointDefinitions(this IServiceCollection services)
    {
        var endpointsDefinitions = new List<IEndpointDefinition>();


        endpointsDefinitions.AddRange(typeof(EndpointDefinitionExtensions).Assembly.ExportedTypes
            .Where(x => typeof(IEndpointDefinition).IsAssignableFrom(x) && !x.IsInterface)
            .Select(Activator.CreateInstance)
            .Cast<IEndpointDefinition>());


        foreach (var endpoint in endpointsDefinitions)
        {
            endpoint.DefineServices(services);
        }

        services.AddSingleton(endpointsDefinitions as IReadOnlyCollection<IEndpointDefinition>);
    }

    public static void AddEndpointDefinitions(this IServiceCollection services, params Type[] scanMarkers)
    {
        var endpointsDefinitions = new List<IEndpointDefinition>();

        foreach (var marker in scanMarkers)
        {
            endpointsDefinitions.AddRange(marker.Assembly.ExportedTypes
                .Where(x => typeof(IEndpointDefinition).IsAssignableFrom(x) && !x.IsInterface &&
                            x.Name.Contains(marker
                                .Name))
                .Select(Activator.CreateInstance)
                .Cast<IEndpointDefinition>());
        }

        foreach (var endpoint in endpointsDefinitions)
        {
            endpoint.DefineServices(services);
        }

        services.AddSingleton(endpointsDefinitions as IReadOnlyCollection<IEndpointDefinition>);
    }

    public static void UseEndpointDefinitions(this WebApplication app)
    {
        var definitions = app.Services.GetRequiredService<IReadOnlyCollection<IEndpointDefinition>>();
        foreach (var endpointDefinition in definitions)
        {
            endpointDefinition.DefineEndpoints(app);
        }
    }
}