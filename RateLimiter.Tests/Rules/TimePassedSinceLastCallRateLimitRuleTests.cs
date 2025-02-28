namespace RateLimiter.Tests.Rules;

using System;
using System.Threading.Tasks;
using NUnit.Framework;
using RateLimiter.Models;
using RateLimiter.Rules;

[TestFixture]
public class TimePassedSinceLastCallRateLimitRuleTests
{
    [Test]
    public async Task IsRequestAllowed_WhenRequiredTimePassed_ReturnsTrue()
    {
        // Arrange
        var rule = new TimePassedSinceLastCallRateLimitRule(TimeSpan.Zero);
        var request = new RequestContext(ResourceId: "resourceA", AccessToken: "123");
        
        // Act
        var result = await rule.IsRequestAllowedAsync(request);
        
        // Assert
        Assert.That(result, Is.True);
    }
    
    [Test]
    public async Task IsRequestAllowed_WhenRequiredTimeNotPassed_ReturnsFalse()
    {
        // Arrange
        var rule = new TimePassedSinceLastCallRateLimitRule(TimeSpan.FromSeconds(1));
        var request = new RequestContext(ResourceId: "resourceA", AccessToken: "123");
        
        // Act
        await rule.IsRequestAllowedAsync(request);
        var result = await rule.IsRequestAllowedAsync(request);
        
        // Assert
        Assert.That(result, Is.False);
    }
}
