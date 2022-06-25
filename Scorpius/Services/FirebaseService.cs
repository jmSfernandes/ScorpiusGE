using System.Diagnostics;
using FirebaseAdmin.Messaging;
using Newtonsoft.Json.Linq;

namespace Scorpius.Services;

public class FirebaseService : IFirebaseService
{
    private readonly INotificationParser _notificationParser;

    public FirebaseService(INotificationParser notificationParser)
    {
        this._notificationParser = notificationParser;
    }

    public async Task<IResult> SendNotification(JObject obj)
    {
        var data = new Dictionary<string, string>();
        string topic = null;
        try
        {
            var shouldSend = _notificationParser.GetValue(obj, "shouldSend");
            if (!Utils.EvaluateExpression(shouldSend))
                return Results.Unauthorized();
            data = new Dictionary<string, string>
            {
                ["type"] = _notificationParser.GetValue(obj, "type"),
                ["message"] = _notificationParser.GetValue(obj, "message"),
                ["time"] = Utils.GetIsoTime(),
                ["data"] = obj.ToString()
            };
            topic = _notificationParser.GetValue(obj, "topic");
        }
        catch (Exception ex)
        {
            return Results.NotFound("There isn't any notification path define for that type of entity");
        }

        // See documentation on defining a message payload.
        var message = new Message()
        {
            Data = data,
            Topic = topic
        };


        // Send a message to the devices subscribed to the provided topic.
        try
        {
            var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            // Response is a message ID string.
            Console.WriteLine("Successfully sent message: " + response);
        }
        catch (Exception)
        {
            return Results.Problem("Something occured while sending your message");
        }

        return Results.Ok();
    }


    public async Task<IResult> SendNotificationMessage(string id, string messageStr)
    {
        // See documentation on defining a message payload.
        var message = new Message()
        {
            Data = new Dictionary<string, string>()
            {
                {"message", messageStr},
            },
            Topic = id
        };

        // Send a message to the devices subscribed to the provided topic.
        try
        {
            var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            // Response is a message ID string.
            Console.WriteLine("Successfully sent message: " + response);
        }
        catch (Exception)
        {
            return Results.Problem("Something occured while sending your message");
        }

        return Results.Ok("");
    }
}