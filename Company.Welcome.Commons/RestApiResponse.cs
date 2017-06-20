using System.Net;

namespace Company.Welcome.Commons
{
    public class RestApiResponse<TResponse>
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public string RestApiMessage { get; set; }
        public TResponse Response { get; set; }
    }
}