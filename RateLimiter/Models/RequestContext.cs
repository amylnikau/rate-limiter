namespace RateLimiter.Models;

public record RequestContext(string ResourceId, string AccessToken, Region ClientRegion = Region.Unknown);
