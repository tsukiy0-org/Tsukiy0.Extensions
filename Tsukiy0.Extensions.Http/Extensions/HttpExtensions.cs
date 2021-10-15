using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Tsukiy0.Extensions.Correlation.Services;
using Tsukiy0.Extensions.Http.Constants;
using Tsukiy0.Extensions.Http.Exceptions;

namespace Tsukiy0.Extensions.Http.Extensions
{
    public static class HttpExtensions
    {
        public static void EnsureStatusCode(this HttpResponseMessage response, IEnumerable<HttpStatusCode> successStatusCodes)
        {
            var isSuccess = successStatusCodes.Contains(response.StatusCode);

            if (!isSuccess)
            {
                throw new HttpException(response);
            }
        }

        public static void EnsureStatusCode(this HttpResponseMessage response)
        {
            response.EnsureStatusCode(new[] { HttpStatusCode.OK });
        }

        public static HttpRequestMessage AddTraceId(this HttpRequestMessage request, ICorrelationService correlationService)
        {
            request.Headers.Add(HttpHeaders.TraceId, correlationService.TraceId.ToString());
            return request;
        }

        public static HttpRequestMessage AddApiKey(this HttpRequestMessage request, string key)
        {
            request.Headers.Add(HttpHeaders.ApiKey, key);
            return request;
        }
    }
}
