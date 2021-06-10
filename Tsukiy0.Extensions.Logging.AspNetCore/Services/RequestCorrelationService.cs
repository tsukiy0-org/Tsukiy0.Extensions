using System;
using Microsoft.AspNetCore.Http;
using Tsukiy0.Extensions.Logging.Core.Services;

namespace Tsukiy0.Extensions.Logging.AspNetCore.Services
{
    public class RequestCorrelationService : ICorrelationService
    {
        public const string HEADER = "x-trace-id";
        private readonly IHttpContextAccessor httpContextAccessor;
        public Guid TraceId => GetTraceId();
        public Guid SpanId { get; }

        public RequestCorrelationService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            SpanId = Guid.NewGuid();
        }

        private Guid GetTraceId()
        {
            var header = httpContextAccessor.HttpContext.Request.Headers[HEADER];

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
