using Newtonsoft.Json.Linq;

namespace Scorpius.Services;

public interface INotificationParser
{
    JObject? GetBodies();
    
    string GetValue(JObject notification, string key);
}