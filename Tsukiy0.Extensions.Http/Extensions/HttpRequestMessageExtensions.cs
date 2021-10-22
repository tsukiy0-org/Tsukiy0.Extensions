using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Tsukiy0.Extensions.Http.Extensions
{
    public static class HttpRequestMessageExtensions
    {
        public static HttpRequestMessage AddBasicAuth(this HttpRequestMessage request, string username, string password)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}")));
            return request;
        }

    }

}

