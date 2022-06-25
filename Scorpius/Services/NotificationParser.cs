using System.Data;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Scorpius.Exceptions;

namespace Scorpius.Services;

public class NotificationParser : INotificationParser
{
    private readonly JObject? _notifications;
    private readonly Regex _rgx;

    public NotificationParser()
    {
        _rgx = new Regex("{.*}", RegexOptions.IgnoreCase);
        var testData = new StreamReader("notifications.json").ReadToEnd();
        _notifications = (JObject) JsonConvert.DeserializeObject(testData);
    }

    public JObject? GetBodies()
    {
        return _notifications;
    }

    private string GetDefaultValue(JObject obj, string key)
    {
        return key switch
        {
            "topic" => obj["id"].ToString(),
            "message" => $"New {obj["type"]}",
            "shouldSend" => "true",
            _ => $"new_{obj["type"]}"
        };
    }

  
    public string GetValue(JObject notification, string key)
    {
        var type = notification["type"]?.ToString();
        if(type==null)
            throw new NoNotificationPathException();
        if (_notifications==null || !_notifications.ContainsKey(type))
            throw new NoNotificationPathException();
        if (!((JObject) _notifications[type]).ContainsKey(key))
            return GetDefaultValue(notification, key);

        var regexInput = _notifications[type]?[key]?.ToString();
        
        var matches = _rgx.Matches(regexInput);
        if (matches.Count == 0)
            return regexInput;
        var regexResult = matches[0].Value;
        var tempKey = regexResult.Replace("{", "").Replace("}", "");
        var replacementStr = notification[tempKey].ToString();

        return regexInput.Replace(regexResult, replacementStr);
    }
}