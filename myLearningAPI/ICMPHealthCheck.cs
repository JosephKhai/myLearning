using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.NetworkInformation;

namespace myLearningAPI;

public class IcmpHealthCheck : IHealthCheck
{
    private readonly int _healthyRoundtripTime;
    private readonly string _host;

    public IcmpHealthCheck(string host, int healthyRoundtripTime)
    {
        _host = host;
        _healthyRoundtripTime = healthyRoundtripTime;
    }


    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var ping = new Ping();
            var reply = await ping.SendPingAsync(_host);

            switch (reply.Status)
            {
                case IPStatus.Success:
                    var msg = $"ICMP to {_host} took {reply.RoundtripTime} ms.";

                    ; return reply.RoundtripTime > _healthyRoundtripTime
                                            ? HealthCheckResult.Degraded(msg)
                                            : HealthCheckResult.Healthy(msg);

                default:
                    var err = $"ICMP to {_host} failed: {reply.Status}";
                    return HealthCheckResult.Unhealthy(err);
            }
        }
        catch (Exception e)
        {
            var err = $"ICMP to {_host} failed: {e.Message}";
            return HealthCheckResult.Unhealthy(err);
        }
    }
}