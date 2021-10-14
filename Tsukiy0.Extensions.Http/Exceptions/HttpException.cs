using System;
using System.Net;
using System.Net.Http;

namespace Tsukiy0.Extensions.Http.Exceptions
{
    public class HttpException : Exception
    {
        public readonly HttpStatusCode StatusCode;

        public HttpException(HttpResponseMessage response) : base($"Request to {response.RequestMessage.RequestUri} failed with status {response.StatusCode}.")
        {
            Data.Add("Method", response.RequestMessage.Method.ToString());
            Data.Add("StatusCode", response.StatusCode.ToString());
            Data.Add("RequestUrl", response.RequestMessage.RequestUri.ToString());
            Data.Add("Content", response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        }
    }
}
