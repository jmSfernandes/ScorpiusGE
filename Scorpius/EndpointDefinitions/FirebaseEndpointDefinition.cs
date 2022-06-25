using Newtonsoft.Json.Linq;
using Scorpius.EndpointDefinitions.Utils;
using Scorpius.Models;
using Scorpius.Services;

namespace Scorpius.EndpointDefinitions;

public class FirebaseEndpointDefinition : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost("/notify", SendNotification)
            .WithName("Notify FCM").WithTags("Notify FCM");
        app.MapPost("/notifyMessage", SendNotification)
            .WithName("Notify FCMMessage").WithTags("Notify FCM");
    }

    public void DefineServices(IServiceCollection services)
    {
        //services.AddScoped<EstimationsService>();
        services.AddScoped<IFirebaseService, FirebaseService>();
        services.AddSingleton<INotificationParser,NotificationParser>();
    }

    //use this if we want to use services instead of direct implementation
    private async Task<IResult> SendNotification(IFirebaseService exampleService, Subscription sub)
    {
        //var id = body["id"]?.ToString();
        var obj = (JObject) JObject.Parse(sub.Data[0].ToString());

        return await exampleService.SendNotification(obj);
    }

    private async Task<IResult> GetSleepData(IFirebaseService exampleService, JObject body)
    {
        var id = body["id"]?.ToString();
        var message = body["message"]?.ToString();

        return await exampleService.SendNotificationMessage(id, message);
    }
}