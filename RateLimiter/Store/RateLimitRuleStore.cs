namespace RateLimiter.Store;

using System.Collections.Generic;
using System.Linq;
using RateLimiter.Rules;

public class RateLimitRuleStore : IRateLimitRuleStore
{
    private static readonly List<IRateLimitRule> EmptyRulesCollection = new();
    private readonly Dictionary<string, List<IRateLimitRule>> _rateLimitRules = new();
    
    public void AddRules(string resourceId, params IRateLimitRule[] rules)
    {
        if (_rateLimitRules.TryGetValue(resourceId, out var existingRules))
        {
            existingRules.AddRange(rules);
            return;
        }
        
        _rateLimitRules.Add(resourceId, rules.ToList());
        
    }

    public IReadOnlyCollection<IRateLimitRule> GetRules(string resourceId)
    {
        return _rateLimitRules.GetValueOrDefault(resourceId, EmptyRulesCollection);
    }
}
