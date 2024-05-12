namespace BinaryQuest.Framework.ModularCore;

/// <summary>
/// if present the output values will be cached in memory for the specified times
/// in generic controllers Lookup Data function
/// </summary>
/// <remarks>
/// if present the output values will be cached in memory for the specified times
/// </remarks>
/// <param name="expiry">expiry in Minute</param>
/// <param name="slidingExpire">Is Sliding Expiry? else Absolute in Min</param>
[System.AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public sealed class LookupCacheAttribute(int expiryInMin, bool slidingExpire = false, string? cacheKey = null) : Attribute
{
    public int Expiry
    {
        get { return expiryInMin; }
    }

    public bool SlidingExpire
    {
        get { return slidingExpire; }
    }

    public string? CacheKey
    {
        get { return cacheKey; }
    }
}
