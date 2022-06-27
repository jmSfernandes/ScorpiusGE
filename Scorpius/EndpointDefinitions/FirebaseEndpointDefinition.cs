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
        app.MapPost("/notifyMessage", SendNotificationMessage)
            .WithName("Notify FCM Simple Message").WithTags("Notify FCM");
    }

    public void DefineServices(IServiceCollection services)
    {
        //services.AddScoped<EstimationsService>();
        services.AddScoped<IFirebaseService, FirebaseService>();
        services.AddSingleton<INotificationParser, NotificationParser>();
    }

    //use this if we want to use services instead of direct implementation
    private async Task<IResult> SendNotification(IFirebaseService firebaseService, Subscription sub)
    {
        //var id = body["id"]?.ToString();
        var obj = JObject.Parse(sub.Data[0].ToString()) ?? null;
        if (obj == null)
            return Results.Problem("The data array should not be empty");
        
        return await firebaseService.SendNotification(obj);
    }

    private async Task<IResult> SendNotificationMessage(IFirebaseService exampleService, SimpleMessage simpleMessage)
    {
        return await exampleService.SendNotificationMessage(simpleMessage.Topic, simpleMessage.Message);
    }
}