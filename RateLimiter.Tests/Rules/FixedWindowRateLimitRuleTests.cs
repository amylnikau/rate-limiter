namespace RateLimiter.Tests.Rules;

using System;
using System.Threading.Tasks;
using NUnit.Framework;
using RateLimiter.Models;
using RateLimiter.Rules;

[TestFixture]
public class FixedWindowRateLimitRuleTests
{
    [Test]
    public async Task IsRequestAllowed_WhenCallCountWithInLimits_ReturnsTrue()
    {
        // Arrange
        var rule = new FixedWindowRateLimitRule(TimeSpan.FromSeconds(1), 10);
        var request = new RequestContext(ResourceId: "resourceA", AccessToken: "123");
        
        // Act
        for (int i = 0; i < 5; i++)
        {
            await rule.IsRequestAllowedAsync(request);
        }
        var result = await rule.IsRequestAllowedAsync(request);
        
        // Assert
        Assert.That(result, Is.True);
    }
    
    [Test]
    public async Task IsRequestAllowed_WhenCallCountExceedsLimits_ReturnsFalse()
    {
        // Arrange
        var rule = new FixedWindowRateLimitRule(TimeSpan.FromSeconds(1), 1);
        var request = new RequestContext(ResourceId: "resourceA", AccessToken: "123");
        
        // Act
        await rule.IsRequestAllowedAsync(request);
        var result = await rule.IsRequestAllowedAsync(request);
        
        // Assert
        Assert.That(result, Is.False);
    }
}
