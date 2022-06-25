using Newtonsoft.Json.Linq;

namespace Scorpius.Models;

public class Subscription
{
    public string? SubscriptionId { get; set; }
    public List<object> Data { get; set; }
}