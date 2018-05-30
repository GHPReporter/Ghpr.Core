using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace Ghpr.CouchDb.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static JObject ContentAsJObject(this HttpResponseMessage message)
        {
            return JObject.Parse(message.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        }
    }
}