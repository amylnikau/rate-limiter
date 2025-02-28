namespace RateLimiter.Tests.Rules;

using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RateLimiter.Models;
using RateLimiter.Rules;

[TestFixture]
public class RegionRateLimitRuleTests
{
    [Test]
    public async Task IsRequestAllowed_WhenRegionMatched_CallsAppropriateRateLimitRule()
    {
        // Arrange
        var request = new RequestContext(ResourceId: "resourceA", AccessToken: "123", ClientRegion: Region.US);
        var rateLimitRule = new Mock<IRateLimitRule>();
        var rule = new RegionRateLimitRule(Region.US, rateLimitRule.Object);

        // Act

        var result = await rule.IsRequestAllowedAsync(request);

        // Assert
        rateLimitRule.Verify(it => it.IsRequestAllowedAsync(request), Times.Once);
    }

    [Test]
    public async Task IsRequestAllowed_WhenRegionDoesNotMatch_ReturnsTrue()
    {
        // Arrange
        var request = new RequestContext(ResourceId: "resourceA", AccessToken: "123", ClientRegion: Region.US);
        var rule = new RegionRateLimitRule(Region.US, new Mock<IRateLimitRule>().Object);

        // Act

        var result = await rule.IsRequestAllowedAsync(request);

        // Assert
        Assert.That(result, Is.True);
    }
}
