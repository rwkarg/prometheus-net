﻿using Microsoft.AspNetCore.Builder;

namespace Prometheus
{
    public static class MetricServerMiddlewareExtensions
    {
        /// <summary>
        /// Starts a Prometheus metrics exporter. The default URL is /metrics, which is a Prometheus convention.
        /// Use static methods on the <see cref="Metrics"/> class to create your metrics.
        /// </summary>
        public static IApplicationBuilder UseMetricServer(this IApplicationBuilder builder, string? url = "/metrics", CollectorRegistry? registry = null, int? port)
        {
            // If there is a URL to map, map it and re-enter without the URL.
            if (url != null)
            {
                if(port.HasValue)
                {
                    return builder.MapWhen(
                        c => c.Connection.LocalPort == port,
                        b0 => b0.Map(path, b1 => b1.UseMetricServer(null, registry)));
                }
                else
                    return builder.Map(url, builder2 => builder2.UseMetricServer(null, registry));
            }

            return builder.UseMiddleware<MetricServerMiddleware>(new MetricServerMiddleware.Settings
            {
                Registry = registry
            });
        }
    }
}
