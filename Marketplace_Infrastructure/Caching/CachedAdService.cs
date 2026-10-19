using Microsoft.Extensions.Caching.Memory;
using Marketplace.Application.Interfaces;
using Marketplace.Application.Dtos;

namespace Marketplace.Infrastructure.Caching;

public class CachedAdService : IAdService
{
    private readonly IAdService _innerService;
    private readonly IMemoryCache _cache;
    private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(5);

    public CachedAdService(IAdService innerService, IMemoryCache cache)
    {
        _innerService = innerService;
        _cache = cache;
    }

    public async Task<AdResponseDto> CreateAdAsync(CreateAdDto dto, CancellationToken ct = default)
    {
        return await _innerService.CreateAdAsync(dto, ct);
    }

    public async Task<AdResponseDto> SubmitForModerationAsync(Guid id, CancellationToken ct = default)
    {
        var result = await _innerService.SubmitForModerationAsync(id, ct);
        
        // Инвалидируем кеш при изменении состояния
        _cache.Remove($"ad:{id}");
        
        return result;
    }

    public async Task<AdResponseDto> ModerateAdAsync(Guid id, bool isApproved, string? reason, CancellationToken ct = default)
    {
        var result = await _innerService.ModerateAdAsync(id, isApproved, reason, ct);
        
        // Инвалидируем кеш при модерации
        _cache.Remove($"ad:{id}");
        
        return result;
    }

    public async Task<AdResponseDto?> GetAdByIdAsync(Guid id, CancellationToken ct = default)
    {
        string cacheKey = $"ad:{id}";

        if (_cache.TryGetValue(cacheKey, out AdResponseDto? cachedAd))
        {
            return cachedAd; // Cache Hit (данные из кеша)
        }

        // Cache Miss (запрос к базе данных через innerService)
        var ad = await _innerService.GetAdByIdAsync(id, ct);
        if (ad != null)
        {
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(CacheTtl);

            _cache.Set(cacheKey, ad, cacheOptions);
        }

        return ad;
    }
}