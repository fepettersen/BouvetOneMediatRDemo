using BouvetOne.MediatR.Core.Common.Caching;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace BouvetOne.MediatR.Core.Infrastructure.Behaviors;

public class CachePipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IDistributedCache _cache;

    public CachePipelineBehavior(IDistributedCache cache) => _cache = cache;

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        if (request is not Cacheable cacheable)
        {
            return await next();
        }

        var cacheKey = cacheable.CacheId();
        var cachedResult = await Get(cacheKey);

        if (cachedResult != null)
        {
            // cached response found, return and short-circuit the pipeline
            return cachedResult;
        }

        var result = await next();
        await Set(result, cacheKey);
        return result;
    }

    public async Task<TResponse?> Get(string cacheKey)
    {
        var response = await _cache.GetStringAsync(cacheKey);
        return string.IsNullOrWhiteSpace(response) ? default : JsonSerializer.Deserialize<TResponse>(response);
    }

    public async Task Set(TResponse value, string cacheKey)
    {
        await _cache.SetStringAsync(
            cacheKey,
            JsonSerializer.Serialize(value),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60), // todo: read from config
                SlidingExpiration = TimeSpan.FromMinutes(20) // todo: read from config
            });
    }
}