using System;

using Microsoft.AspNetCore.Http;

using Tsukiy0.Extensions.Correlation.Services;
using Tsukiy0.Extensions.Http.Constants;

namespace Tsukiy0.Extensions.AspNetCore.Services
{
    public class RequestCorrelationService : ICorrelationService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public Guid TraceId { get; }
        public Guid SpanId { get; }

        public RequestCorrelationService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            SpanId = Guid.NewGuid();
            TraceId = GetTraceId();
        }

        private Guid GetTraceId()
        {
            var header = httpContextAccessor.HttpContext.Request.Headers[HttpHeaders.TraceId];

            if (header.Count == 0)
            {
                return Guid.NewGuid();
            }

            try
            {
                return new Guid(header[0]);
            }
            catch
            {
                return Guid.NewGuid();
            }
        }
    }
}