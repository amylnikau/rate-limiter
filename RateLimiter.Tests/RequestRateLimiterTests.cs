namespace RateLimiter.Tests;

using System;
using System.Threading.Tasks;
using NUnit.Framework;
using RateLimiter.Models;
using RateLimiter.Rules;
using RateLimiter.Store;

[TestFixture]
public class RequestRateLimiterTests
{
    [Test]
    public async Task IsRequestAllowed_WhenAllRateLimitRulesPass_ReturnsTrue()
    {
        // Arrange
        var rateLimiterStore = new RateLimitRuleStore();
        rateLimiterStore.AddRules(
            "resourceA", 
            new TimePassedSinceLastCallRateLimitRule(TimeSpan.FromMinutes(1)),
            new RegionRateLimitRule(Region.EU, new FixedWindowRateLimitRule(TimeSpan.FromMinutes(1), 5)),
            new RegionRateLimitRule(Region.US, new FixedWindowRateLimitRule(TimeSpan.FromMinutes(1), 100)));
        var rateLimiter = new RequestRateLimiter(rateLimiterStore);

        var request = new RequestContext(ResourceId: "resourceA", AccessToken: "123", ClientRegion: Region.EU);

        // Act
        var result = await rateLimiter.IsRequestAllowedAsync(request);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task IsRequestAllowed_WhenAnyLimitRuleFails_ReturnsFalse()
    {
        // Arrange
        var rateLimiterStore = new RateLimitRuleStore();
        rateLimiterStore.AddRules(
            "resourceA",
            new TimePassedSinceLastCallRateLimitRule(TimeSpan.FromMinutes(5)),
            new RegionRateLimitRule(Region.US, new FixedWindowRateLimitRule(TimeSpan.FromMinutes(1), 100)),
            new RegionRateLimitRule(Region.EU, new TimePassedSinceLastCallRateLimitRule(TimeSpan.FromMinutes(1))));
        var rateLimiter = new RequestRateLimiter(rateLimiterStore);

        var request = new RequestContext(ResourceId: "resourceA", AccessToken: "123", ClientRegion: Region.EU);

        // Act
        for (int i = 0; i < 10; i++)
        {
            await rateLimiter.IsRequestAllowedAsync(request);
        }

        var result = await rateLimiter.IsRequestAllowedAsync(request);

        // Assert
        Assert.That(result, Is.False);
    }
    
    [Test]
    public async Task IsRequestAllowed_WhenNoRulesConfigured_ReturnsTrue()
    {
        // Arrange
        var rateLimiterStore = new RateLimitRuleStore();
        rateLimiterStore.AddRules("resourceA", new TimePassedSinceLastCallRateLimitRule(TimeSpan.FromMinutes(1)));
        var rateLimiter = new RequestRateLimiter(rateLimiterStore);

        var request = new RequestContext(ResourceId: "resourceB", AccessToken: "123");

        // Act
        var result = await rateLimiter.IsRequestAllowedAsync(request);

        // Assert
        Assert.That(result, Is.True);
    }
}
