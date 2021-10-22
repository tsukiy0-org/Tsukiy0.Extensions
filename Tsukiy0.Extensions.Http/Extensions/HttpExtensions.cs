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
    }
}
