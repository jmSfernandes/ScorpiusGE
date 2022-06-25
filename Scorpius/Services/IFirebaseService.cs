using Newtonsoft.Json.Linq;

namespace Scorpius.Services;

public interface IFirebaseService
{
    Task<IResult> SendNotification(JObject obj);
    Task<IResult> SendNotificationMessage(string id, string message);
}