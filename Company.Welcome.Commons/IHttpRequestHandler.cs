using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Company.Welcome.Commons
{
    public interface IHttpRequestHandler
    {
        Task<RestApiResponse<TResponse>> PerformRequest<TResponse, TRequest>(
            HttpRequestHandler.HttpMethod httpMethod,
            Uri apiPath,
            IDictionary<string, string> headers,
            string contentType,
            TRequest json = default(TRequest));

        Task<RestApiResponse<TResponse>> PerformRequest<TResponse>(
            HttpRequestHandler.HttpMethod httpMethod,
            Uri apiPath,
            IDictionary<string, string> headers,
            string contentType);
    }
}