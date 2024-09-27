using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using SmartShop.Services.RequestProvider;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly:Dependency(typeof(RequestProvider))]
namespace SmartShop.Services.RequestProvider
{
    public class RequestProvider : IRequestProvider
    {
        private readonly JsonSerializerSettings _serializerSettings;

        private readonly Lazy<HttpClient> _httpClient =
            new Lazy<HttpClient>(() =>
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                return httpClient;
            },
                LazyThreadSafetyMode.ExecutionAndPublication);

        public RequestProvider()
        {
            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore,
            };
            _serializerSettings.Converters.Add(new StringEnumConverter());
        }

        public async Task<TResult> GetAsync<TResult>(string uri, string token = "")
        {
            HttpClient httpClient = GetOrCreateHttpClient(token);
            HttpResponseMessage response = await httpClient.GetAsync(uri).ConfigureAwait(false);

            await RequestProvider.HandleResponse(response).ConfigureAwait(false);

            string serialized = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            TResult result = JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings);

            return result;
        }

        public async Task<PResult> PostAsync<TResult, PResult>(string uri, TResult data, string token = "", string header = "")
        {
            HttpClient httpClient = GetOrCreateHttpClient(token);

            if (!string.IsNullOrEmpty(header))
            {
                RequestProvider.AddHeaderParameter(httpClient, header);
            }

            var content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PostAsync(uri, content).ConfigureAwait(false);

            await RequestProvider.HandleResponse(response).ConfigureAwait(false);
            string serialized = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            PResult result = JsonConvert.DeserializeObject<PResult>(serialized, _serializerSettings);

            return result;
        }

        public async Task<PResult> PutAsync<TResult, PResult>(string uri, TResult data, string token = "", string header = "")
        {
            HttpClient httpClient = GetOrCreateHttpClient(token);

            if (!string.IsNullOrEmpty(header))
            {
                RequestProvider.AddHeaderParameter(httpClient, header);
            }

            var content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PutAsync(uri, content).ConfigureAwait(false);

            await RequestProvider.HandleResponse(response).ConfigureAwait(false);
            string serialized = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            PResult result = JsonConvert.DeserializeObject<PResult>(serialized, _serializerSettings);

            return result;
        }

        public async Task DeleteAsync(string uri, string token = "")
        {
            HttpClient httpClient = GetOrCreateHttpClient(token);
            await httpClient.DeleteAsync(uri).ConfigureAwait(false);
        }

        private HttpClient GetOrCreateHttpClient(string token = "")
        {
            var httpClient = _httpClient.Value;

            httpClient.DefaultRequestHeaders.Authorization =
                !string.IsNullOrEmpty(token)
                    ? new AuthenticationHeaderValue("Bearer", token)
                    : null;

            return httpClient;
        }

        private static void AddHeaderParameter(HttpClient httpClient, string parameter)
        {
            if (httpClient == null)
                return;

            if (string.IsNullOrEmpty(parameter))
                return;

            httpClient.DefaultRequestHeaders.Add(parameter, Guid.NewGuid().ToString());
        }

        private static async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode == HttpStatusCode.Forbidden ||
                        response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new ServiceAuthenticationException(content);
                }

                throw new HttpRequestExceptionEx(response.StatusCode, content);
            }
        }
    }
}
