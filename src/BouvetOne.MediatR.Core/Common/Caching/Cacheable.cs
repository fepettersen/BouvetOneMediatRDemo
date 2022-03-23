namespace BouvetOne.MediatR.Core.Common.Caching;

public abstract class Cacheable
{
    public virtual string CacheId() => this.GetType().Name;
}