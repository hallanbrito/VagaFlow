using System.Net;
using System.Net.Sockets;

namespace VagaFlow.Api.Jobs;

/// <summary>
/// Pure URL validation/canonicalization. Never follows links or resolves DNS.
/// OriginalUri remains untouched for the candidate to open in their own browser.
/// </summary>
public static class JobUrl
{
    private static readonly HashSet<string> TrackingParameters =
        new(StringComparer.OrdinalIgnoreCase) { "fbclid", "gclid", "msclkid", "igshid" };

    public static bool TryNormalize(string? rawUrl, out string? canonicalUrl)
    {
        canonicalUrl = null;
        if (string.IsNullOrWhiteSpace(rawUrl) || rawUrl.Length > 2048 ||
            !Uri.TryCreate(rawUrl.Trim(), UriKind.Absolute, out var uri) ||
            uri.Scheme != Uri.UriSchemeHttps ||
            string.IsNullOrWhiteSpace(uri.Host) ||
            !string.IsNullOrEmpty(uri.UserInfo) ||
            IsUnsafeHost(uri.Host))
        {
            return false;
        }

        var queryParameters = uri.Query.TrimStart('?')
            .Split('&', StringSplitOptions.RemoveEmptyEntries)
            .Where(parameter =>
            {
                var key = parameter.Split('=', 2)[0];
                return !key.StartsWith("utm_", StringComparison.OrdinalIgnoreCase) &&
                       !TrackingParameters.Contains(key);
            })
            .OrderBy(parameter => parameter, StringComparer.Ordinal)
            .ToArray();

        var canonical = new UriBuilder(uri)
        {
            Host = uri.IdnHost.ToLowerInvariant(),
            Fragment = string.Empty,
            Query = string.Join("&", queryParameters)
        };

        if (canonical.Uri.IsDefaultPort)
        {
            canonical.Port = -1;
        }

        canonicalUrl = canonical.Uri.AbsoluteUri.TrimEnd('/');
        return canonicalUrl.Length <= 2048;
    }

    private static bool IsUnsafeHost(string host)
    {
        var normalized = host.TrimEnd('.').ToLowerInvariant();

        if (normalized == "localhost" ||
            normalized.EndsWith(".localhost", StringComparison.Ordinal) ||
            normalized.EndsWith(".local", StringComparison.Ordinal) ||
            normalized.EndsWith(".internal", StringComparison.Ordinal) ||
            !normalized.Contains('.'))
        {
            return true;
        }

        if (!IPAddress.TryParse(normalized, out var address))
        {
            return false;
        }

        if (IPAddress.IsLoopback(address) ||
            address.IsIPv6LinkLocal ||
            address.IsIPv6SiteLocal ||
            address.Equals(IPAddress.IPv6Any) ||
            address.Equals(IPAddress.Any))
        {
            return true;
        }

        if (address.AddressFamily == AddressFamily.InterNetworkV6)
        {
            if (address.IsIPv4MappedToIPv6)
            {
                address = address.MapToIPv4();
            }
            else
            {
                return (address.GetAddressBytes()[0] & 0xFE) == 0xFC;
            }
        }

        var bytes = address.GetAddressBytes();
        return bytes[0] == 0 ||
               bytes[0] == 10 ||
               bytes[0] == 127 ||
               (bytes[0] == 169 && bytes[1] == 254) ||
               (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31) ||
               (bytes[0] == 192 && bytes[1] == 168);
    }
}
