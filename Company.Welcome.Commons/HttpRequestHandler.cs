using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;

namespace Company.Welcome.Commons
{
    public class HttpRequestHandler : IHttpRequestHandler
    {
        public HttpRequestHandler(ISerializer serializer, ILogger logger)
        {
            _serializer = serializer;
            _logger = logger;
        }

        private readonly ISerializer _serializer;
        private readonly ILogger _logger;

        public enum HttpMethod
        {
            Get,
            Post,
            Delete,
            Put
        }

        private async Task<RestApiResponse<TResponse>> PerformRequestInternal<TResponse>(
            Func<HttpClient, Task<HttpResponseMessage>> actionToPerform)
        {
            var response = new RestApiResponse<TResponse>()
            {
                Response = default(TResponse)
            };
            using (var httpClient = new HttpClient() { Timeout = new TimeSpan(0, 0, 30) })
            {
                try
                {
                    var httpResponseMessage = await actionToPerform(httpClient);
                    if (httpResponseMessage != null)
                    {
                        response.HttpStatusCode = httpResponseMessage.StatusCode;
                        if (httpResponseMessage.IsSuccessStatusCode)
                        {
                            var content = await httpResponseMessage.Content.ReadAsStringAsync();
                            var entity = _serializer.Deserialize<TResponse>(content);
                            response.Response = entity;
                        }
                        else
                        {
                            response.RestApiMessage = httpResponseMessage.ReasonPhrase;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Log(ex);
                    throw;
                }
            }
            return response;
        }

        public async Task<RestApiResponse<TResponse>> PerformRequest<TResponse, TRequest>(HttpMethod httpMethod,
            Uri apiPath,
            IDictionary<string, string> headers,
            string contentType,
            TRequest json = default(TRequest))
        {
            return await PerformRequestInternal<TResponse>(async httpClient =>
            {
                var httpResponseMessage = default(HttpResponseMessage);
                switch (httpMethod)
                {
                    case HttpMethod.Post:
                        var serialized = string.Empty;
                        if (json != null)
                        {
                            serialized = _serializer.Serialize<TRequest>(json);
                        }
                        var requestStream = new StringContent(serialized, System.Text.Encoding.UTF8, contentType);
                        httpResponseMessage = await httpClient.PostAsync(apiPath, requestStream).ConfigureAwait(false);
                        break;
                    case HttpMethod.Delete:
                        break;
                    case HttpMethod.Put:
                        break;
                    case HttpMethod.Get:
                    default:
                        httpResponseMessage = await httpClient.GetAsync(apiPath).ConfigureAwait(false);
                        break;
                }
                return httpResponseMessage;
            });
        }

        public async Task<RestApiResponse<TResponse>> PerformRequest<TResponse>(HttpMethod httpMethod, 
            Uri apiPath, 
            IDictionary<string, string> headers, 
            string contentType)
        {
            return await PerformRequestInternal<TResponse>(async httpClient =>
            {
                var httpResponseMessage = default(HttpResponseMessage);
                switch (httpMethod)
                {
                    case HttpMethod.Post:
                        var serialized = string.Empty;
                        var requestStream = new StringContent(serialized, System.Text.Encoding.UTF8, contentType);
                        httpResponseMessage = await httpClient.PostAsync(apiPath, requestStream).ConfigureAwait(false);
                        break;
                    case HttpMethod.Delete:
                        break;
                    case HttpMethod.Put:
                        break;
                    case HttpMethod.Get:
                    default:
                        httpResponseMessage = await httpClient.GetAsync(apiPath).ConfigureAwait(false);
                        break;
                }
                return httpResponseMessage;
            });
        }
    }
}