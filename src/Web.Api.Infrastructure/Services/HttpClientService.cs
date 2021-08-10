using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using NLog;
using Web.Api.Core.Interfaces.Services;

namespace Web.Api.Infrastructure.Services
{
    public class HttpClientService : IHttpClientService
    {
        public async Task<string> Post(string url, string bodyData)
        {
            using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), url))
                    {
                        request.Headers.TryAddWithoutValidation("accept", "*/*"); 

                        request.Content = new StringContent(bodyData);
                        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json-patch+json"); 

                        var response = await httpClient.SendAsync(request);

                        if(response.IsSuccessStatusCode)
                        {
                            var responseBody = await response.Content.ReadAsStringAsync();
                            return responseBody;
                        }
                        return null;
                    }
                }    
        }

        public async Task<string> Put(string url, string bodyData)
        {
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("PUT"), url))
                {
                    request.Headers.TryAddWithoutValidation("accept", "*/*"); 

                    request.Content = new StringContent(bodyData);
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json-patch+json"); 

                    var response = await httpClient.SendAsync(request);
                    if(response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        return responseBody;
                    }
                    return null;
                }
            } 
        }

        public async Task<string> Get(string url)
        {
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), url))
                {
                    request.Headers.TryAddWithoutValidation("accept", "*/*"); 

                    var response = await httpClient.SendAsync(request);
                    if(response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        return responseBody;
                    }
                    return null;
                }
            } 
        }

        public async Task<string> Delete(string url, string bodyData)
        {
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("DELETE"), url))
                {
                    request.Headers.TryAddWithoutValidation("accept", "*/*"); 

                    request.Content = new StringContent(bodyData);
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json-patch+json"); 

                    var response = await httpClient.SendAsync(request);
                    if(response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        return responseBody;
                    }
                    return null;
                }
            } 
        }
    }
}