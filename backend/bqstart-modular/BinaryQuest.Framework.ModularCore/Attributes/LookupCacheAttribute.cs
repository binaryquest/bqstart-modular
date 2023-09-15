namespace BinaryQuest.Framework.ModularCore;

/// <summary>
/// if present the output values will be cached in memory for the specified times
/// in genertic controllers Lookup Data function
/// </summary>
[System.AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public sealed class LookupCacheAttribute : Attribute
{
    readonly int expiry;
    readonly bool slidingExpire;

    /// <summary>
    /// if present the output values will be cached in memory for the specified times
    /// </summary>
    /// <param name="expiry">expiry in Minute</param>
    /// <param name="slidingExpire">Is Sliding Expirty? else Absolute in Min</param>
    public LookupCacheAttribute(int expiryInMin, bool slidingExpire = false)
    {
        this.expiry = expiryInMin;
        this.slidingExpire = slidingExpire;
    }

    public int Expiry
    {
        get { return expiry; }
    }

    public bool SlidingExpire
    {
        get { return slidingExpire; }
    }        
}
