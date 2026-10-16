using System.Collections.Concurrent;
using Marketplace.Domain.Entities;
using Marketplace.Domain.Repositories;

namespace Marketplace.Infrastructure.Persistence;

public class InMemoryAdRepository : IAdRepository
{
    private readonly ConcurrentDictionary<Guid, Ad> _storage = new();

    public Task<Ad?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _storage.TryGetValue(id, out var ad);
        return Task.FromResult(ad);
    }

    public Task AddAsync(Ad ad, CancellationToken cancellationToken = default)
    {
        _storage[ad.Id] = ad;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Ad ad, CancellationToken cancellationToken = default)
    {
        _storage[ad.Id] = ad;
        return Task.CompletedTask;
    }
}