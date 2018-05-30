using System.Net.Http;

namespace Ghpr.CouchDb.Extensions
{
    public static class HttpClientExtensions
    {
        public static HttpResponseMessage Post(this HttpClient client, string uri, HttpContent content)
        {
            return client.PostAsync(uri, content).GetAwaiter().GetResult();
        }

        public static HttpResponseMessage Find(this HttpClient client, string databaseName, HttpContent content)
        {
            return client.PostAsync($"/{databaseName}/_find", content).GetAwaiter().GetResult();
        }

        public static HttpResponseMessage Purge(this HttpClient client, string databaseName, HttpContent content)
        {
            return client.PostAsync($"/{databaseName}/_purge", content).GetAwaiter().GetResult();
        }

        public static HttpResponseMessage Put(this HttpClient client, string uri, HttpContent content)
        {
            return client.PutAsync(uri, content).GetAwaiter().GetResult();
        }
        
        public static HttpResponseMessage Delete(this HttpClient client, string uri)
        {
            return client.DeleteAsync(uri).GetAwaiter().GetResult();
        }

        public static string GetString(this HttpClient client, string uri)
        {
            return client.GetStringAsync(uri).GetAwaiter().GetResult();
        }
    }
}