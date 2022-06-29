using System.Diagnostics;
using FirebaseAdmin.Messaging;
using Newtonsoft.Json.Linq;

namespace Scorpius.Services;

public class FirebaseService : IFirebaseService
{
    private readonly INotificationParser _notificationParser;
    private readonly ILogger<FirebaseService> _logger;

    public FirebaseService(INotificationParser notificationParser, ILogger<FirebaseService> logger)
    {
        _notificationParser = notificationParser;
        _logger = logger;
    }

    public async Task<IResult> SendNotification(JObject obj)
    {
        var data = new Dictionary<string, string>();
        string topic;
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
            _logger.LogInformation("Successfully sent message: {Response}", response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Something occured while sending your message: {Exception}", ex.Message);
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
            _logger.LogInformation("Successfully sent message: {Response}", response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Something occured while sending your message: {Exception}", ex.Message);
            return Results.Problem("Something occured while sending your message");
        }

        return Results.Ok("");
    }
}