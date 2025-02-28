namespace RateLimiter.Rules;

using System.Threading.Tasks;
using RateLimiter.Models;

public interface IRateLimitRule
{
    Task<bool> IsRequestAllowedAsync(RequestContext request);
}
