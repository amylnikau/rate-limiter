namespace RateLimiter;

using System.Threading.Tasks;
using RateLimiter.Models;

public interface IRateLimiter
{
    Task<bool> IsRequestAllowedAsync(RequestContext request);
}
