using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Tsukiy0.Extensions.Correlation.Services;

namespace Tsukiy0.Extensions.Http.Extensions
{
    public static class HttpRequestMessageExtensions
    {
        public static HttpRequestMessage AddBasicAuth(this HttpRequestMessage request, string username, string password)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}")));
            return request;
        }

        public static HttpRequestMessage AddUserAgent(this HttpRequestMessage request, ProductInfoHeaderValue value)
        {
            request.Headers.UserAgent.Add(value);
            return request;
        }

        public static HttpRequestMessage AddHeader(this HttpRequestMessage request, string key, string value)
        {
            request.Headers.Add(key, value);
            return request;
        }

        public static HttpRequestMessage AddTraceId(this HttpRequestMessage request, ICorrelationService correlationService)
        {
            return request.AddHeader(Constants.HttpHeaders.TraceId, correlationService.TraceId.ToString());
        }

        public static HttpRequestMessage AddApiKey(this HttpRequestMessage request, string key)
        {
            return request.AddHeader(Constants.HttpHeaders.ApiKey, key);
        }
    }
}

